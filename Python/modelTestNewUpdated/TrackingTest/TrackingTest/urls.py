
from django.contrib import admin
from django.http import HttpResponse
from django.urls import path, include
from rest_framework.routers import DefaultRouter
from modtest.views import (
     CustomTokenObtainView,
     MyTokenObtainPairView,
     RegisterView,
     create_opportunity,
     delete_opportunity,
     download_report,
     get_account_details,
     hello,
     get_opportunity,
     update_opportunity,
     partial_update_opportunity
 )

from django.urls import reverse_lazy
from django.urls import path
from django.urls import re_path
from rest_framework_simplejwt.views import TokenRefreshView

router = DefaultRouter()

urlpatterns = [
    path('admin/', admin.site.urls),
    path('', include(router.urls)),
    path('sample/',hello,name="hello"),
    re_path(r'^opportunity/(?P<pk>\d+)?/?$', get_opportunity, name='get_opportunity'),
    re_path(r'^get_account_details/(?P<pk>\d+)?/?$', get_account_details, name='get_account_details'),
    path('opportunities/create/', create_opportunity, name='opportunity-create'),
    path('opportunities/update/<int:pk>/', update_opportunity, name='opportunity-update'),
    path('opportunities/partial-update/<int:pk>/', partial_update_opportunity, name='opportunity-partial-update'),
    path('opportunities/delete/<int:pk>/', delete_opportunity, name='opportunity-delete'),
    path('opportunities/download_report/', download_report, name='opportunity-download-report'),
    path('api/token/', CustomTokenObtainView.as_view(), name='custom_token_obtain'),
    path('api/token/refresh/', TokenRefreshView.as_view(), name='token_refresh'),
    path('api/register/', RegisterView.as_view(), name='register'),

]
