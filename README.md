# CZERTAINLY Authorization Service

> This repository is part of the commercial open source project CZERTAINLY. You can find more information about the project at [CZERTAINLY](https://github.com/3KeyCompany/CZERTAINLY) repository, including the contribution guide.

`Auth` service in Czertainly platform is designed as central service for managing access control to different resources (entities) in Czertainly platform and authenticate users based on authentication type.

`Auth` service offers following functionality:
- authentication of user
- management of roles
- management of users and their roles membership
- management of resources (entities) and their actions
- management of roles permissions for specific resources and their actions

User can belong to multiple roles and thus permissions of users are merged from permissions of its individual roles permissions.

## Authentication flow

Currently, there are 3 user authentication types in `Auth` service based on data in payload of authentication endpoint `/auth`:
- X.509 certificate - certificate content is passed as URL encoded BASE64 string   
- JSON authentication token
- Username of system user

After successful authentication, user details with its merged role permissions is returned. If none of above specified data are present, user is authenticated as anonymous user with limited permissions only for internal purposes.

### X.509 Certificate

When authenticating user with certificate, its content string is decoded and parsed.
Afterwards it is verified if it is valid and trusted.
Then, based on its fingerprint, it is mapped to user from database and return authentication response.

### JSON authentication token

When authenticating user with JSON auth token, it is deserialized and must conform to the required structure.
In this authentication type, username specified in token is used as unique information based on which user is mapped.
If user with specified username does not exist, based on configuration of `auth` service, user can be automatically created with information from token.
Same can be applied for user's roles. Based on configuration, also roles can be automatically created. Existing roles specified in token are assigned to user.

### Username of system user

This authentication type is used only for internal authentication of system users to elevate permissions and perform actions that are otherwise subject to authorization.

## Docker container

`Auth` service is provided as a Docker container. Use the `3keycompany/czertainly-auth:tagname` to pull the required image from the repository. It can be configured using the following environment variables:

| Variable                               | Description                                                                         | Required | Default value |
|----------------------------------------|-------------------------------------------------------------------------------------|----------|---------------|
| `ConnectionStrings__DefaultConnection` | Connection string for database access                                               | Yes      | N/A           |
| `AuthOptions__CreateUnknownUsers`      | Unknown user with username specified in auth token will be created                  | No       | false         |
| `AuthOptions__CreateUnknownRoles`      | Unknown role with name specified in auth token will be created and assigned to user | No       | false         |
