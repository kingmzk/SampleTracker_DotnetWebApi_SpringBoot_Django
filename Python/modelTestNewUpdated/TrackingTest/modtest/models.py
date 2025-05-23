



from django.db import models

class Accelerator(models.Model):
    id = models.AutoField(primary_key=True)
    value = models.CharField(max_length=255)

    def __str__(self):
        return self.value

    class Meta:
        db_table = "accelerator"


class Competition(models.Model):
    id = models.AutoField(primary_key=True)
    value = models.CharField(max_length=100, unique=True)   

    def __str__(self):
        return self.value

    class Meta:
        db_table = "competition"


class Microservice(models.Model):
    id = models.AutoField(primary_key=True)
    value = models.CharField(max_length=100)

    def __str__(self):
        return self.value

    class Meta:
        db_table = "microservice"


class OpportunityTracker(models.Model):
    TECHNOLOGY_CHOICES = [
        ('AI', 'Artificial Intelligence'),
        ('ML', 'Machine Learning'),
        ('Blockchain', 'Blockchain'),
        ('Cloud', 'Cloud Computing'),
    ]
    id = models.AutoField(primary_key=True)
    op_id = models.IntegerField(unique=True)
    op_name = models.CharField(max_length=255)
    team_size = models.FloatField(null=True, blank=True)
    client_name = models.CharField(max_length=255)
    technology = models.CharField(max_length=50, choices=TECHNOLOGY_CHOICES, blank=True, null=True)
    client_genai_adoption = models.ForeignKey(
        "ClientGenAIAdoptation", on_delete=models.PROTECT, null=True, blank=True
    )
    reason_for_genai_adoption = models.ForeignKey(
        "ReasonForGenAIAdoptation", on_delete=models.PROTECT, null=True, blank=True
    )
    accelerators = models.ManyToManyField(Accelerator, through="OppAccelerator")
    competitions = models.ManyToManyField(Competition, through="OppCompetition")
    microservices = models.ManyToManyField(Microservice, through="OppMicroservice")
    account = models.ForeignKey("account_list", on_delete=models.PROTECT, null=True, blank=True)
    parent = models.ForeignKey('self', on_delete=models.SET_NULL, null=True, blank=True, related_name='child_opp')





    def __str__(self):
        return self.op_name

    class Meta:
        db_table = "opportunity_tracker"


class sales_lead(models.Model):
    id = models.AutoField(primary_key=True)
    ps_no = models.CharField(max_length=255, null=True, blank=True)
    name = models.CharField(max_length=255, null=True, blank=True)
    email = models.EmailField(max_length=255, null=True, blank=True)
    uan = models.CharField(max_length=255, null=True, blank=True)
    def __str__(self):
        return self.name
    
    class Meta:
        db_table = "sales_lead"



class sbu(models.Model):
    id = models.AutoField(primary_key=True)
    value = models.CharField(max_length=255, unique=True, blank=True)
    sales_lead = models.ManyToManyField(sales_lead, through="sbu_sales_lead_rel")
    
    def __str__(self):
        return self.value

    class Meta:
        db_table = "sbu"


class pph(models.Model):
    id = models.AutoField(primary_key=True)
    ps_no = models.CharField(max_length=255, null=True, blank=True)
    name = models.CharField(max_length=255, null=True, blank=True)
    email = models.EmailField(max_length=255, null=True, blank=True)
    uan = models.CharField(max_length=255, null=True, blank=True)

    def __str__(self):
        return self.name
    
    class Meta:
        db_table = "pph"


class delivery_unit(models.Model):
    id = models.AutoField(primary_key=True)
    value = models.CharField(max_length=255, unique=True, blank=True)
    du_code  = models.TextField(max_length=255, null=True, blank=True)
    pph = models.ManyToManyField(pph, through="du_pph_rel")
    
    def __str__(self):
        return self.value

    class Meta:
        db_table = "delivery_unit"


class du_pph_rel(models.Model):
    id = models.AutoField(primary_key=True)
    du_id = models.ForeignKey(delivery_unit, on_delete=models.PROTECT, null=True, blank=True)
    pph_id = models.ForeignKey(pph, on_delete=models.PROTECT, null=True, blank=True)
   
    class Meta:
        db_table = "du_pph_rel"


class sbu_sales_lead_rel(models.Model):
    id = models.AutoField(primary_key=True)
    sbu_id = models.ForeignKey(sbu, on_delete=models.PROTECT, null=True, blank=True)
    sales_lead_id = models.ForeignKey(sales_lead, on_delete=models.PROTECT, null=True, blank=True)
   
    class Meta:
        db_table = "sbu_sales_lead_rel"



class account_list(models.Model):
    id = models.AutoField(primary_key=True)
    value = models.CharField(max_length=255, unique=True, blank=True)
    ig = models.CharField(max_length=255, null=True, blank=True)
    sbu = models.ForeignKey(sbu, on_delete=models.PROTECT, null=True, blank=True)
    delivery_unit = models.ForeignKey(delivery_unit, on_delete=models.PROTECT, null=True, blank=True)
    account_id = models.IntegerField(null=True, blank=True)
    customer_group_name = models.CharField(max_length=255, null=True, blank=True)

    class Meta:
        db_table = "account_list"



class ClientGenAIAdoptation(models.Model):
    id = models.AutoField(primary_key=True)
    value = models.CharField(max_length=255, unique=True, blank=True)
    is_disabled = models.BooleanField(null=True, blank=True)

    def __str__(self):
        return self.value

    class Meta:
        db_table = "client_genai_adoptation"


class ReasonForGenAIAdoptation(models.Model):
    id = models.AutoField(primary_key=True)
    value = models.CharField(max_length=255, unique=True, blank=True)
    is_disabled = models.BooleanField(null=True, blank=True)

    def __str__(self):
        return self.value

    class Meta:
        db_table = "reason_for_no_genai_adoption"


class OppAccelerator(models.Model):
    id = models.AutoField(primary_key=True)
    opportunity = models.ForeignKey(OpportunityTracker, on_delete=models.PROTECT)
    accelerator = models.ForeignKey(Accelerator, on_delete=models.PROTECT)

    class Meta:
        db_table = "opp_accelerator"


class OppCompetition(models.Model):
    id = models.AutoField(primary_key=True)
    opportunity = models.ForeignKey(OpportunityTracker, on_delete=models.PROTECT)
    competition = models.ForeignKey(Competition, on_delete=models.PROTECT)

    class Meta:
        db_table = "opp_competition"


class OppMicroservice(models.Model):
    id = models.AutoField(primary_key=True)
    opportunity = models.ForeignKey(OpportunityTracker, on_delete=models.PROTECT)
    microservice = models.ForeignKey(Microservice, on_delete=models.PROTECT)

    class Meta:
        db_table = "opp_microservice"





"""
SELECT 
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
LEFT JOIN microservice m ON om.microservice_id = m.id
ORDER BY ot.id;
"""