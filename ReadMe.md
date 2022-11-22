
Run RabbitMQ:
docker run --rm -it -p 15672:15672 -p 5672:5672 rabbitmq:3-management

Run Sql Server:
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=str0ngp@ssword" -e "MSSQL_PID=Express" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest

Run MongoDB:
docker run -d -p 27017:27017 --name example-mongo mongo:latest
