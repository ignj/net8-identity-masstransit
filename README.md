# NET 8 Pub-Sub

Very simple set of applications to test the Identity features from NET 8 and the Publisher Subscriber mechanisms using MassTransit and RabbitMQ.

# How to use
Run `docker compose up --build`  
Migrations are applied automatically to a PSQL database.

## Create User
```
curl --location --request POST 'http://localhost:5000/register' \
--header 'Content-Type: application/json' \
--data-raw '{
    "username": "test",
    "password": "Test1!",
    "email": "test@test.com"
}'
```

## Login
```
curl --location --request POST 'http://localhost:5000/login' \
--header 'Content-Type: application/json' \
--data-raw '{
    "email": "test@test.com",
    "password": "Test1!"
}'
```

## Ping with registered user
```
curl --location --request GET 'http://localhost:5000/ping-auth' \
--header 'Authorization: Bearer {token_from_login_endpoint}'
```
The consumer service should display something like `Received ping: Ping 10/25/2023 12:31:15 - test@test.com`

## Ping anonymously
```
curl --location --request GET 'http://localhost:5000/ping'
```
The consumer service should display something like `Received ping: Ping 10/25/2023 12:30:07`
