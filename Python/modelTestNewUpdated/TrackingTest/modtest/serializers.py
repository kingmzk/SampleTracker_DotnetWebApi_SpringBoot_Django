from rest_framework import serializers
from .models import (
    Accelerator,
    Competition,
    Microservice,
    OpportunityTracker,
    ClientGenAIAdoptation,
    ReasonForGenAIAdoptation,
    OppAccelerator,
    OppCompetition,
    OppMicroservice,
    account_list,
    delivery_unit,
    pph,
    sales_lead,
    sbu
)

class AcceleratorSerializer(serializers.ModelSerializer):
    class Meta:
        model = Accelerator
        fields = ['id', 'value']


class CompetitionSerializer(serializers.ModelSerializer):
    class Meta:
        model = Competition
        fields = ['id', 'value']


class MicroserviceSerializer(serializers.ModelSerializer):
    class Meta:
        model = Microservice
        fields = ['id', 'value']


class ClientGenAIAdoptationSerializer(serializers.ModelSerializer):
    class Meta:
        model = ClientGenAIAdoptation
        fields = ['id', 'value', 'is_disabled']


class ReasonForGenAIAdoptationSerializer(serializers.ModelSerializer):
    class Meta:
        model = ReasonForGenAIAdoptation
        fields = ['id', 'value', 'is_disabled']


class OpportunityTrackerSerializer(serializers.ModelSerializer):
    client_genai_adoption = ClientGenAIAdoptationSerializer()
    reason_for_genai_adoption = ReasonForGenAIAdoptationSerializer()
    accelerators = AcceleratorSerializer(many=True, read_only=True)
    competitions = CompetitionSerializer(many=True, read_only=True)
    microservices = MicroserviceSerializer(many=True, read_only=True)

    class Meta:
        model = OpportunityTracker
        fields = [
            'id',
            'op_id',
            'op_name',
            'team_size',
            'technology',
            'client_name',
            'client_genai_adoption',
            'reason_for_genai_adoption',
            'accelerators',
            'competitions',
            'microservices',
            'account',
            'parent'
        ]



class OppAcceleratorSerializer(serializers.ModelSerializer):
    class Meta:
        model = OppAccelerator
        fields = ['id', 'opportunity', 'accelerator']


class OppCompetitionSerializer(serializers.ModelSerializer):
    class Meta:
        model = OppCompetition
        fields = ['id', 'opportunity', 'competition']


class OppMicroserviceSerializer(serializers.ModelSerializer):
    class Meta:
        model = OppMicroservice
        fields = ['id', 'opportunity', 'microservice']


class SalesLeadSerializer(serializers.ModelSerializer):
    class Meta:
        model = sales_lead
        fields = ['id', 'ps_no', 'name', 'email', 'uan']

class PphSerializer(serializers.ModelSerializer):
    class Meta:
        model = pph
        fields = ['id', 'ps_no', 'name', 'email', 'uan']

class SbuSerializer(serializers.ModelSerializer):
    sales_lead = SalesLeadSerializer(many=True, read_only=True)

    class Meta:
        model = sbu
        fields = ['id', 'value', 'sales_lead']

class DeliveryUnitSerializer(serializers.ModelSerializer):
    pph = PphSerializer(many=True, read_only=True)

    class Meta:
        model = delivery_unit
        fields = ['id', 'value', 'du_code', 'pph']


class AccountListSerializer(serializers.ModelSerializer):
    sbu = SbuSerializer(read_only=True)
    delivery_unit = DeliveryUnitSerializer(read_only=True)
    class Meta:
        model = account_list
        fields = [
            'id', 'value', 'ig', 'account_id', 'customer_group_name',
            'sbu', 'delivery_unit'
        ]

