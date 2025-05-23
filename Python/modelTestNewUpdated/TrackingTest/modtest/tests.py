from django.test import TestCase
from django.urls import reverse
from rest_framework import status
from rest_framework.test import APIClient
from .models import OptyTracker
from .serializers import OptyTrackerSerializer

class GetOpportunityTestCase(TestCase):
    def setUp(self):
        self.client = APIClient()

    def test_get_opportunity(self):
        optytracker = OptyTracker.objects.create(op_id=1, op_name='Test Opportunity', client_name='Client 1')

        response = self.client.get(reverse('get_opportunity', kwargs={'pk': optytracker.pk}))
        self.assertEqual(response.status_code, status.HTTP_200_OK)

        serializer = OptyTrackerSerializer(optytracker)
        self.assertEqual(response.data, serializer.data)
