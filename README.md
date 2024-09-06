# UserManagementApiSolution

✔ Debes entregar el código fuente por medio del repositorio de tu preferencia.

Se entrega código en GitHub

✔ Podrás usar cualquier motor de bases de datos para el almacenamiento.

Se utiliza contenedor de SqlServer

  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    ports:
      - "1433:1433"
    environment:
      - SA_PASSWORD=Your_password123
      - ACCEPT_EULA=Y
    volumes:
      - sqlserverdata:/var/opt/mssql

✔ Deberás usar arquitectura de software orientada al dominio.

Arquitectura Solucion:

UserManagement.Api
Actúa como la interfaz con el exterior. Aquí se exponen los endpoints de la API que los clientes consumirán.

UserManagement.Application
Contiene la lógica de aplicación y coordina las operaciones de alto nivel entre la infraestructura y el dominio. Aquí se implementan los casos de uso específicos de la aplicación, como registrarse, iniciar sesión, realizar pedidos, etc.

UserManagement.Domain
Corazón de la solución DDD. Incluye todas las entidades, objetos de valor, agregados, eventos de dominio, y lógica de negocio que representan el modelo de negocio central. Este proyecto no debe tener dependencias hacia afuera, manteniendo la pureza del dominio.
UserManagement.Infrastructure

Implementa la lógica para acceder a bases de datos, sistemas de archivos, colas de mensajes, etc. Aquí se implementan los repositorios que manejan la recuperación y persistencia de los objetos del dominio.

![image](https://github.com/user-attachments/assets/17a21181-904b-4ed9-a474-4dd04e1dc667)


✔ Usa .NET 8 o 9 para desarrollar los microservicios.

Se utilizó .NET 8

✔ Implementa un mecanismo de logging y monitorización para los microservicios.

Para el tema de Logging se utilizo SeriLog 
// Configuración de Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/myapp.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();
Para el tema monitirzación se utilizo Prometheus y Grafana

Prometheus

![image](https://github.com/user-attachments/assets/ef5fe88d-2633-4a27-b6a2-c435127a21ee)

Grafana

![image](https://github.com/user-attachments/assets/1c73288d-727b-458e-83cd-bf62fc3fbbd4)


// Configuración de Prometheus para exposición de métricas
app.UseHttpMetrics();

 prometheus:
   image: prom/prometheus
   ports:
     - "9090:9090"
   volumes:
     - ./prometheus.yml:/etc/prometheus/prometheus.yml
   command:
     - '--config.file=/etc/prometheus/prometheus.yml'
   
 grafana:
   image: grafana/grafana
   ports:
     - "3000:3000"
     
✔ Implementa pruebas unitarias y de integración para los endpoints principales
 Pruebas Unitario se creo proyecto UserManagement.Api.Tests.csproj
 Pruebas de Integración se creo proyecto UserManagement.Api.IntegrationTests.csproj
 
✔ Documenta la API usando Swagger
  Se documento la api en Swagger

  ![image](https://github.com/user-attachments/assets/5dc18f28-13b4-4664-9c94-1c9972940ba1)
  
  
✔ Uso adecuado de .NET Core y patrones de diseño. 
  Se realizo uso adecuado de .Net Core y de patrones de diseño
  
✔ Seguridad en la API (autenticación y autorización).
  Se utilizo JWT para asegurar el RestApi
  ![image](https://github.com/user-attachments/assets/73b72403-a4d2-4697-a17c-458f108822a7)

  
✔ Despliegue utilizando Kubernetes y configuración de CI/CD
  No tengo el servico ACK para iniciar la publicación de los contenedores


