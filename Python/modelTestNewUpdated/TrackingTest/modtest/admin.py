from django.contrib import admin
from .models import (
    Accelerator, 
    Competition, 
    Microservice, 
    OpportunityTracker, 
    OppMicroservice, 
    OppCompetition, 
    OppAccelerator, 
    ReasonForGenAIAdoptation, 
    ClientGenAIAdoptation,
    sales_lead, sbu, pph, delivery_unit,
    du_pph_rel, sbu_sales_lead_rel, account_list
)

@admin.register(Accelerator)
class AcceleratorAdmin(admin.ModelAdmin):
    list_display = ['id', 'value']

@admin.register(Competition)
class CompetitionAdmin(admin.ModelAdmin):
    list_display = ['id', 'value']

@admin.register(Microservice)
class MicroserviceAdmin(admin.ModelAdmin):
    list_display = ['id', 'value']

@admin.register(OpportunityTracker)
class OpportunityTrackerAdmin(admin.ModelAdmin):
    list_display = ["id", "op_id","op_name","team_size","client_name","technology"]

@admin.register(OppAccelerator)
class OppAcceleratorAdmin(admin.ModelAdmin):
    list_display = ['id', 'opportunity', 'accelerator']  
@admin.register(OppCompetition)
class OppCompetitionAdmin(admin.ModelAdmin):
    list_display = ['id', 'opportunity', 'competition']  

@admin.register(OppMicroservice)
class OppMicroserviceAdmin(admin.ModelAdmin):
    list_display = ['id', 'opportunity', 'microservice']  

@admin.register(ClientGenAIAdoptation)
class ClientGenAIAdoptionAdmin(admin.ModelAdmin):
    list_display = ['id', 'value', 'is_disabled']

@admin.register(ReasonForGenAIAdoptation)
class ReasonForNoGenAIAdoptionAdmin(admin.ModelAdmin):
    list_display = ['id', 'value', 'is_disabled']


# Inline for SBU - Sales Lead relationship
class SbuSalesLeadInline(admin.TabularInline):
    model = sbu_sales_lead_rel
    extra = 1

# Inline for Delivery Unit - PPH relationship
class DuPphInline(admin.TabularInline):
    model = du_pph_rel
    extra = 1

@admin.register(sales_lead)
class SalesLeadAdmin(admin.ModelAdmin):
    list_display = ('id', 'name', 'ps_no', 'email', 'uan')
    search_fields = ('name', 'ps_no', 'email', 'uan')
    inlines = [SbuSalesLeadInline]

@admin.register(sbu)
class SbuAdmin(admin.ModelAdmin):
    list_display = ('id', 'value')
    search_fields = ('value',)
    inlines = [SbuSalesLeadInline]

@admin.register(pph)
class PphAdmin(admin.ModelAdmin):
    list_display = ('id', 'name', 'ps_no', 'email', 'uan')
    search_fields = ('name', 'ps_no', 'email', 'uan')

@admin.register(delivery_unit)
class DeliveryUnitAdmin(admin.ModelAdmin):
    list_display = ('id', 'value', 'du_code')
    search_fields = ('value', 'du_code')
    inlines = [DuPphInline]

@admin.register(account_list)
class AccountListAdmin(admin.ModelAdmin):
    list_display = ('id', 'value', 'ig', 'account_id', 'customer_group_name', 'sbu', 'delivery_unit')
    search_fields = ('value', 'ig', 'customer_group_name')
    list_filter = ('sbu', 'delivery_unit')

@admin.register(du_pph_rel)
class DuPphRelAdmin(admin.ModelAdmin):
    list_display = ('id', 'du_id', 'pph_id')

@admin.register(sbu_sales_lead_rel)
class SbuSalesLeadRelAdmin(admin.ModelAdmin):
    list_display = ('id', 'sbu_id', 'sales_lead_id')