from django.http import JsonResponse

def hello(request):
    return JsonResponse({"message" : "Hello world this message is not connected to any database"})

from django.shortcuts import get_object_or_404
from rest_framework.decorators import api_view
from rest_framework.response import Response
from rest_framework import status
from .models import OpportunityTracker, Accelerator, Competition, Microservice, ClientGenAIAdoptation, ReasonForGenAIAdoptation, account_list
from .serializers import (
    AccountListSerializer,
    OpportunityTrackerSerializer,
    AcceleratorSerializer,
    CompetitionSerializer,
    MicroserviceSerializer,
    ClientGenAIAdoptationSerializer,
    ReasonForGenAIAdoptationSerializer
)

@api_view(['GET'])
def get_opportunity(request, pk=None):
    if pk is not None:
        try:
            opt = OpportunityTracker.objects.get(pk=pk)
            serializer = OpportunityTrackerSerializer(opt)
            return Response(serializer.data, status=status.HTTP_200_OK)
        except OpportunityTracker.DoesNotExist:
            return Response({"error": "Opportunity not found"}, status=status.HTTP_404_NOT_FOUND)
    else:
        queryset = OpportunityTracker.objects.all()
        serializer = OpportunityTrackerSerializer(queryset, many=True)
        return Response(serializer.data, status=status.HTTP_200_OK)
    



# from django.http import JsonResponse
# from .models import account_list

# @api_view(['GET'])
# def get_account_details(request, pk=None):
#     def serialize_account(account):
#         return {
#             "id": account.id,
#             "value": account.value,
#             "ig": account.ig,
#             "account_id": account.account_id,
#             "customer_group_name": account.customer_group_name,
#             "sbu": {
#                 "id": account.sbu.id if account.sbu else None,
#                 "value": account.sbu.value if account.sbu else None,
#                 "sales_lead": [
#                     {
#                         "id": sl.id,
#                         "ps_no": sl.ps_no,
#                         "name": sl.name,
#                         "email": sl.email,
#                         "uan": sl.uan
#                     } for sl in account.sbu.sales_lead.all()
#                 ] if account.sbu else []
#             },
#             "delivery_unit": {
#                 "id": account.delivery_unit.id if account.delivery_unit else None,
#                 "value": account.delivery_unit.value if account.delivery_unit else None,
#                 "du_code": account.delivery_unit.du_code if account.delivery_unit else None,
#                 "pph": [
#                     {
#                         "id": p.id,
#                         "ps_no": p.ps_no,
#                         "name": p.name,
#                         "email": p.email,
#                         "uan": p.uan
#                     } for p in account.delivery_unit.pph.all()
#                 ] if account.delivery_unit else []
#             }
#         }

#     if pk is not None:
#         try:
#             account = account_list.objects.select_related('sbu', 'delivery_unit')\
#                                           .prefetch_related('sbu__sales_lead', 'delivery_unit__pph')\
#                                           .get(pk=pk)
#             data = serialize_account(account)
#             return JsonResponse(data, safe=False, status=200)
#         except account_list.DoesNotExist:
#             return JsonResponse({"error": "Account not found"}, status=404)
#     else:
#         accounts = account_list.objects.select_related('sbu', 'delivery_unit')\
#                                        .prefetch_related('sbu__sales_lead', 'delivery_unit__pph').all()
#         data = [serialize_account(acc) for acc in accounts]
#         return JsonResponse(data, safe=False, status=200)


@api_view(['GET'])
def get_account_details(request, pk=None):
    if pk is not None:
        try:
            account = account_list.objects.get(pk=pk)
            print(account)
            serializer = AccountListSerializer(account)
            return Response(serializer.data, status=status.HTTP_200_OK)
        except account_list.DoesNotExist:
            return Response({"error": "Account not found"}, status=status.HTTP_404_NOT_FOUND)
    else:
        queryset = account_list.objects.all()
        serializer = AccountListSerializer(queryset, many=True)
        return Response(serializer.data, status=status.HTTP_200_OK)





from django.core.exceptions import ObjectDoesNotExist

@api_view(['POST'])
def create_opportunity(request):
    data = request.data
    try:
        # Resolve ForeignKey fields using string values
        client_genai_adoption = ClientGenAIAdoptation.objects.get(value=data.get("client_genai_adoption"))
        reason_for_genai_adoption = ReasonForGenAIAdoptation.objects.get(value=data.get("reason_for_genai_adoption"))

        # Create OpportunityTracker instance
        opportunity = OpportunityTracker.objects.create(
            op_id=data["op_id"],
            op_name=data["op_name"],
            team_size=data.get("team_size"),
            client_name=data["client_name"],
            client_genai_adoption=client_genai_adoption,
            reason_for_genai_adoption=reason_for_genai_adoption,
        )

        # Handle Many-to-Many relationships
        try:
            opportunity.accelerators.set([
                Accelerator.objects.get(value=accel) for accel in data.get("accelerators", [])
            ])
        except ObjectDoesNotExist:
            return Response({"error": f"One or more provided accelerators are invalid: {data['accelerators']}"}, 
                            status=status.HTTP_400_BAD_REQUEST)

        try:
            opportunity.competitions.set([
                Competition.objects.get(value=comp) for comp in data.get("competitions", [])
            ])
        except ObjectDoesNotExist:
            return Response({"error": f"One or more provided competitions are invalid: {data['competitions']}"}, 
                            status=status.HTTP_400_BAD_REQUEST)

        try:
            opportunity.microservices.set([
                Microservice.objects.get(value=ms) for ms in data.get("microservices", [])
            ])
        except ObjectDoesNotExist:
            return Response({"error": f"One or more provided microservices are invalid: {data['microservices']}"}, 
                            status=status.HTTP_400_BAD_REQUEST)

        serializer = OpportunityTrackerSerializer(opportunity)
        return Response(serializer.data, status=status.HTTP_201_CREATED)

    except ClientGenAIAdoptation.DoesNotExist:
        return Response({"error": f"ClientGenAIAdoptation value '{data.get('client_genai_adoption')}' does not exist"}, 
                        status=status.HTTP_400_BAD_REQUEST)
    except ReasonForGenAIAdoptation.DoesNotExist:
        return Response({"error": f"ReasonForGenAIAdoptation value '{data.get('reason_for_genai_adoption')}' does not exist"}, 
                        status=status.HTTP_400_BAD_REQUEST)
    except Exception as e:
        return Response({"error": str(e)}, status=status.HTTP_400_BAD_REQUEST)


@api_view(['PUT'])
def update_opportunity(request, pk):
    try:
        opportunity = get_object_or_404(OpportunityTracker, pk=pk)
        data = request.data

        # Update ForeignKey fields using string values
        if "client_genai_adoption" in data:
            try:
                opportunity.client_genai_adoption = ClientGenAIAdoptation.objects.get(value=data["client_genai_adoption"])
            except ClientGenAIAdoptation.DoesNotExist:
                return Response({"error": f"ClientGenAIAdoptation value '{data['client_genai_adoption']}' does not exist"}, 
                                status=status.HTTP_400_BAD_REQUEST)

        if "reason_for_genai_adoption" in data:
            try:
                opportunity.reason_for_genai_adoption = ReasonForGenAIAdoptation.objects.get(value=data["reason_for_genai_adoption"])
            except ReasonForGenAIAdoptation.DoesNotExist:
                return Response({"error": f"ReasonForGenAIAdoptation value '{data['reason_for_genai_adoption']}' does not exist"}, 
                                status=status.HTTP_400_BAD_REQUEST)

        # Update other fields
        opportunity.op_id = data.get("op_id", opportunity.op_id)
        opportunity.op_name = data.get("op_name", opportunity.op_name)
        opportunity.team_size = data.get("team_size", opportunity.team_size)
        opportunity.client_name = data.get("client_name", opportunity.client_name)

        if "technology" in data:
            technology_value = data["technology"]
            if technology_value not in dict(OpportunityTracker.TECHNOLOGY_CHOICES):
                return Response({"error": f"Invalid technology '{technology_value}'"}, 
                                 status=status.HTTP_400_BAD_REQUEST)
            opportunity.technology = technology_value
        opportunity.save()

        # Update Many-to-Many relationships
        if "accelerators" in data:
            try:
                opportunity.accelerators.set([Accelerator.objects.get(value=accel) for accel in data["accelerators"]])
            except Accelerator.DoesNotExist:
                return Response({"error": f"One or more provided accelerators are invalid: {data['accelerators']}"}, 
                                status=status.HTTP_400_BAD_REQUEST)

        if "competitions" in data:
            try:
                opportunity.competitions.set([Competition.objects.get(value=comp) for comp in data["competitions"]])
            except Competition.DoesNotExist:
                return Response({"error": f"One or more provided competitions are invalid: {data['competitions']}"}, 
                                status=status.HTTP_400_BAD_REQUEST)

        if "microservices" in data:
            try:
                opportunity.microservices.set([Microservice.objects.get(value=ms) for ms in data["microservices"]])
            except Microservice.DoesNotExist:
                return Response({"error": f"One or more provided microservices are invalid: {data['microservices']}"}, 
                                status=status.HTTP_400_BAD_REQUEST)

        serializer = OpportunityTrackerSerializer(opportunity)
        return Response(serializer.data, status=status.HTTP_200_OK)

    except Exception as e:
        return Response({"error": str(e)}, status=status.HTTP_400_BAD_REQUEST)



@api_view(['PATCH'])
def partial_update_opportunity(request, pk):
    try:
        opportunity = get_object_or_404(OpportunityTracker, pk=pk)
        data = request.data

        # Update ForeignKey fields if provided
        if "client_genai_adoption" in data:
            try:
                opportunity.client_genai_adoption = ClientGenAIAdoptation.objects.get(value=data["client_genai_adoption"])
            except ClientGenAIAdoptation.DoesNotExist:
                return Response({"error": f"ClientGenAIAdoptation value '{data['client_genai_adoption']}' does not exist"}, 
                                status=status.HTTP_400_BAD_REQUEST)

        if "reason_for_genai_adoption" in data:
            try:
                opportunity.reason_for_genai_adoption = ReasonForGenAIAdoptation.objects.get(value=data["reason_for_genai_adoption"])
            except ReasonForGenAIAdoptation.DoesNotExist:
                return Response({"error": f"ReasonForGenAIAdoptation value '{data['reason_for_genai_adoption']}' does not exist"}, 
                                status=status.HTTP_400_BAD_REQUEST)

        # Update other fields if provided
        opportunity.op_id = data.get("op_id", opportunity.op_id)
        opportunity.op_name = data.get("op_name", opportunity.op_name)
        opportunity.team_size = data.get("team_size", opportunity.team_size)
        opportunity.client_name = data.get("client_name", opportunity.client_name)

        
        if "technology" in data:
            technology_value = data["technology"]
            if technology_value not in dict(OpportunityTracker.TECHNOLOGY_CHOICES):
                return Response({"error": f"Invalid technology '{technology_value}'"}, 
                                 status=status.HTTP_400_BAD_REQUEST)
            opportunity.technology = technology_value

        opportunity.save()

        # Update Many-to-Many relationships if provided
        if "accelerators" in data:
            try:
                opportunity.accelerators.set([Accelerator.objects.get(value=accel) for accel in data["accelerators"]])
            except Accelerator.DoesNotExist:
                return Response({"error": f"One or more provided accelerators are invalid: {data['accelerators']}"}, 
                                status=status.HTTP_400_BAD_REQUEST)

        if "competitions" in data:
            try:
                opportunity.competitions.set([Competition.objects.get(value=comp) for comp in data["competitions"]])
            except Competition.DoesNotExist:
                return Response({"error": f"One or more provided competitions are invalid: {data['competitions']}"}, 
                                status=status.HTTP_400_BAD_REQUEST)

        if "microservices" in data:
            try:
                opportunity.microservices.set([Microservice.objects.get(value=ms) for ms in data["microservices"]])
            except Microservice.DoesNotExist:
                return Response({"error": f"One or more provided microservices are invalid: {data['microservices']}"}, 
                                status=status.HTTP_400_BAD_REQUEST)

        serializer = OpportunityTrackerSerializer(opportunity)
        return Response(serializer.data, status=status.HTTP_200_OK)

    except Exception as e:
        return Response({"error": str(e)}, status=status.HTTP_400_BAD_REQUEST)


    

@api_view(['DELETE'])
def delete_opportunity(request, pk):
    try:
        # Fetch the OpportunityTracker object with the primary key
        opportunity = get_object_or_404(OpportunityTracker, pk=pk)

        # Delete many-to-many relationships through intermediate models
        opportunity.accelerators.clear()  # Removes relationships but does not delete Accelerator objects
        opportunity.competitions.clear()  # Removes relationships but does not delete Competition objects
        opportunity.microservices.clear()  # Removes relationships but does not delete Microservice objects

        # Optionally, delete foreign key relationships if you want to cascade delete
        # If you want the related objects in foreign keys to be deleted (optional, depending on business rules)
        if opportunity.client_genai_adoption:
            opportunity.client_genai_adoption = None
            opportunity.save()

        if opportunity.reason_for_genai_adoption:
            opportunity.reason_for_genai_adoption = None
            opportunity.save()

        # Finally, delete the OpportunityTracker instance
        opportunity.delete()

        return Response({"message": "Opportunity Tracker and related data deleted successfully"}, status=status.HTTP_204_NO_CONTENT)

    except OpportunityTracker.DoesNotExist:
        return Response({"error": "Opportunity Tracker not found"}, status=status.HTTP_404_NOT_FOUND)
    except Exception as e:
        return Response({"error": str(e)}, status=status.HTTP_400_BAD_REQUEST)



import pyodbc
from django.http import HttpResponse
from io import BytesIO
import openpyxl
from rest_framework.decorators import api_view
from rest_framework import status  # Added missing import
from rest_framework.response import Response  # Added for proper error responses

@api_view(['POST'])
def download_report(request):
    try:
        connection_string = (
            "DRIVER={ODBC Driver 17 for SQL Server};"
            "SERVER=KINGMZK\\SQLEXPRESS;"
            "DATABASE=pydjango;"
            "UID=kingmzk;"
            "PWD=kingmzk"
        )

        with pyodbc.connect(connection_string) as conn:
            with conn.cursor() as cursor:
                base_query = """SELECT 
                    ot.*,
                    cga.value AS client_genai_adoption,
                    rga.value AS reason_for_no_genai_adoption,
                    a.value AS accelerator,
                    c.value AS competition,
                    m.value AS microservice
                    FROM opportunity_tracker ot
                    LEFT JOIN client_genai_adoptation cga ON ot.client_genai_adoption_id = cga.id
                    LEFT JOIN reason_for_no_genai_adoption rga ON ot.reason_for_genai_adoption_id = rga.id
                    LEFT JOIN opp_accelerator oa ON ot.id = oa.opportunity_id
                    LEFT JOIN accelerator a ON oa.accelerator_id = a.id
                    LEFT JOIN opp_competition oc ON ot.id = oc.opportunity_id
                    LEFT JOIN competition c ON oc.competition_id = c.id
                    LEFT JOIN opp_microservice om ON ot.id = om.opportunity_id
                    LEFT JOIN microservice m ON om.microservice_id = m.id"""
                
                where_clauses = []
                params = []

                if request.data.get('isFilterApplied', False):
                    filters = request.data.get('filters', {})
                    
                    # Name filter
                    if filters.get('name', '').strip():
                        where_clauses.append("ot.op_name LIKE ?")
                        params.append(f"%{filters['name'].strip()}%")
                    
                    # Client name filter
                    if filters.get('clientname', '').strip():
                        where_clauses.append("ot.client_name LIKE ?")
                        params.append(f"%{filters['clientname'].strip()}%")
                    
                    # Team size filter
                    teamsize = filters.get('teamsize')
                    if teamsize is not None and str(teamsize).strip() != '':
                        try:
                            where_clauses.append("ot.team_size = ?")
                            params.append(float(teamsize))
                        except ValueError:
                            return Response(
                                {"error": "Invalid team size value"}, 
                                status=status.HTTP_400_BAD_REQUEST
                            )

                # Build final query
                final_query = base_query
                if where_clauses:
                    final_query += f" WHERE {' AND '.join(where_clauses)}"
                final_query += " ORDER BY ot.id;"

                # Execute the CORRECTED FINAL QUERY with parameters
                cursor.execute(final_query, params)  # Fixed: Use final_query instead of base_query
                columns = [column[0] for column in cursor.description]
                data = cursor.fetchall()

        # Create Excel file
        excel_file = BytesIO()
        workbook = openpyxl.Workbook()
        sheet = workbook.active
        
        # Add headers
        sheet.append(columns)
        
        # Add data rows
        for row in data:
            sheet.append(list(row))
        
        workbook.save(excel_file)
        excel_file.seek(0)

        response = HttpResponse(
            excel_file.read(),
            content_type='application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
        )
        response['Content-Disposition'] = 'attachment; filename="opportunity_report.xlsx"'
        return response

    except Exception as e:
        return HttpResponse(f"Error generating report: {str(e)}", status=500)