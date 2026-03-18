import requests
import json
from typing import Dict, Any

BASE_URL = "http://localhost:5152"  # Update if needed

USERS = {
    "admin": {"username": "admin", "password": "Admin@123"},
    "branch_manager": {"username": "mgr_muscat", "password": "Manager@123"},
    "customer": {"username": "cust_ahmed", "password": "Customer@123"}
}
TOKENS = {}

# Utility functions

def login(user_key: str) -> str:
    print(f"Logging in as {user_key}...")
    url = f"{BASE_URL}/api/Auth/login"
    data = {"username": USERS[user_key]["username"], "password": USERS[user_key]["password"]}
    resp = requests.post(url, json=data)
    assert resp.status_code == 200, f"Login failed for {user_key}: {resp.text}"
    token = resp.json()["data"]["token"]
    TOKENS[user_key] = token
    print(f"Logged in as {user_key}.")
    return token

def auth_headers(user_key: str) -> Dict[str, str]:
    return {"Authorization": f"Bearer {TOKENS[user_key]}"}

# Test helpers

def print_action(desc):
    print(f"\nACTION: {desc}")

def print_result(resp, expected_status, desc):
    if resp.status_code == expected_status:
        print(f"PASS: {desc} [{resp.status_code}]")
    else:
        print(f"FAIL: {desc} [{resp.status_code}]\nResponse: {resp.text}")

# Public endpoints

def test_public_branch_listing():
    print_action("Listing branches (public endpoint)")
    resp = requests.get(f"{BASE_URL}/api/Branch?skip=0&take=10")
    print_result(resp, 200, "Public branch listing")

# Auth endpoints

def test_auth_cycle():
    print_action("Registering customer (should fail for duplicate user)")
    resp = requests.post(f"{BASE_URL}/api/Auth/register", files={
        "Username": (None, USERS["customer"]["username"]),
        "Password": (None, USERS["customer"]["password"]),
        "FullName": (None, "Ahmed Al Harthy"),
        "Email": (None, "ahmed.h@example.com"),
        "Phone": (None, "+96890000001"),
        "IdImage": ("id.png", b"0"*1024*1024, "image/png")
    })
    print_result(resp, 400, "Register duplicate customer")

    login("customer")
    print_action("Refreshing customer token")
    resp = requests.post(f"{BASE_URL}/api/Auth/refresh-token", headers=auth_headers("customer"), json={"userId": 1})
    print_result(resp, 200, "Refresh token")

    print_action("Logging out customer")
    resp = requests.post(f"{BASE_URL}/api/Auth/logout", headers=auth_headers("customer"))
    print_result(resp, 200, "Logout customer")

# Protected endpoints

def test_protected_branch_listing():
    login("admin")
    print_action("Listing branches as admin (protected endpoint)")
    resp = requests.get(f"{BASE_URL}/api/Branch?skip=0&take=10", headers=auth_headers("admin"))
    print_result(resp, 200, "Admin branch listing")

    login("customer")
    print_action("Customer attempts to list branches (should be allowed if public, forbidden if protected)")
    resp = requests.get(f"{BASE_URL}/api/Branch?skip=0&take=10", headers=auth_headers("customer"))
    if resp.status_code == 200:
        print("PASS: Customer branch listing (public endpoint)")
    else:
        print_result(resp, 403, "Customer forbidden branch listing")

# Input validation

def test_invalid_branch_creation():
    login("admin")
    print_action("Creating branch with invalid data (empty name)")
    resp = requests.post(f"{BASE_URL}/api/Branch", headers=auth_headers("admin"), json={"name": "", "city": "", "address": "", "timezone": ""})
    print_result(resp, 400, "Invalid branch creation")

# Business logic

def test_duplicate_branch_creation():
    login("admin")
    print_action("Creating branch with duplicate data")
    resp = requests.post(f"{BASE_URL}/api/Branch", headers=auth_headers("admin"), json={"name": "BranchTest", "city": "Muscat", "address": "Test Address", "timezone": "Asia/Muscat"})
    print_result(resp, 400, "Duplicate branch creation")

# Edge cases

def test_not_found_customer_file():
    login("admin")
    print_action("Getting customer file for non-existent reference")
    resp = requests.get(f"{BASE_URL}/api/File/customer/999", headers=auth_headers("admin"))
    print_result(resp, 404, "Customer file not found")

# Authorization

def test_admin_only_endpoint():
    login("admin")
    print_action("Accessing admin-only endpoint as admin")
    resp = requests.get(f"{BASE_URL}/api/AuditLog?skip=0&take=10", headers=auth_headers("admin"))
    print_result(resp, 200, "Admin audit log access")

    login("customer")
    print_action("Accessing admin-only endpoint as customer (should be forbidden)")
    resp = requests.get(f"{BASE_URL}/api/AuditLog?skip=0&take=10", headers=auth_headers("customer"))
    print_result(resp, 403, "Customer forbidden audit log access")

# Massive test runner
def main():
    test_public_branch_listing()
    test_auth_cycle()
    test_protected_branch_listing()
    test_invalid_branch_creation()
    test_duplicate_branch_creation()
    test_not_found_customer_file()
    test_admin_only_endpoint()
    print("\nAll massive test cases completed.")

if __name__ == "__main__":
    main()
