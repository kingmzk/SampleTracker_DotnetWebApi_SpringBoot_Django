from django.contrib.auth.backends import BaseBackend
from modtest.models import User
from django.contrib.auth.hashers import check_password
from rest_framework.permissions import BasePermission

class CustomUserAuthBackend(BaseBackend):
    def authenticate(self, request, username=None, password=None, **kwargs):
        try:
            user = User.objects.get(username=username)
            if check_password(password, user.password):
                return user
        except User.DoesNotExist:
            return None

    def get_user(self, user_id):
        try:
            return User.objects.get(pk=user_id)
        except User.DoesNotExist:
            return None

class IsAdminRole(BasePermission):
    def has_permission(self, request, view):
        return request.user.is_authenticated and getattr(request.user.role, "id", None) == 1


class IsUserRole(BasePermission):

    def has_permission(self, request, view):
        return request.user.is_authenticated and getattr(request.user.role, "id", None) == 2