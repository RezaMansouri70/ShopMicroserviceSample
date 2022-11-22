
<b>For Run On Docker</b><br>
Run All Services On Docker Use Docker Compose Up Command In  Root Project Thath Has docker-compose.yml File
<br>
docker-compose up
<br>

<b>For Run Separatly On Docker</b>
<br>
<b> Run RabbitMQ: </b> <br>
docker run --rm -it -p 15672:15672 -p 5672:5672 rabbitmq:3-management  <br>
<b> Run Sql Server: </b> <br>
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=str0ngp@ssword" -e "MSSQL_PID=Express" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest  <br>

<b> Run MongoDB:</b> <br>
docker run -d -p 27017:27017 --name example-mongo mongo:latest<br>
