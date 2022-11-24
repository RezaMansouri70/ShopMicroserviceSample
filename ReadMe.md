
For See How To Run And More Information Pelase See The Article

<h2> <a href="https://medium.com/@rezamansouri/implementing-microservice-architecture-in-net-part-1-project-overview-2d94b79607e3" target="_blank" > Implementing Microservice Architecture In .NET </a>  </h2>


<b>For Run On Docker</b><br>
Run All Services On Docker Use Docker Compose Up Command In  Root Project That Has docker-compose.yml File
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
