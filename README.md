Sistema de Gestión de Reservas Oscar Castro

Instrucciones de Configuración
1. Descargar y Abrir el Proyecto
Descarga el código fuente del proyecto y descomprímelo en tu máquina.
Abre el archivo de solución (.sln) en Visual Studio.
2. Configurar la Base de Datos
Dentro de la carpeta del proyecto encontrarás un archivo llamado DatabaseScript.sql. Este archivo contiene todos los comandos necesarios para crear y poblar la base de datos.
Abre SQL Server Management Studio (SSMS) o tu herramienta de administración preferida.
Ejecuta el script DatabaseScript.sql en dos partes primera parte hasta la linea "-- Stored Procedures for Salas"para crear la base de datos y las tablas requeridas y la segunda parte para crear los Store Procedures requeridos.
3. Modificar la Cadena de Conexión
El proyecto utiliza una cadena de conexión específica en el archivo Web.config:
<connectionStrings>
   <add name="DefaultConnection" connectionString="Server=DESKTOP-0S3UA1G;Database=ReservationManagementSystem;Trusted_Connection=True;" providerName="System.Data.SqlClient" />
</connectionStrings>
Pasos para Ajustar la Cadena de Conexión
Abre el archivo Web.config en la raíz del proyecto.

Localiza el bloque <connectionStrings> y modifica la propiedad connectionString para que apunte a tu servidor SQL Server y base de datos.
Ejemplo para autenticación de Windows:
<connectionStrings>
   <add name="DefaultConnection" connectionString="Server=MI_SERVIDOR;Database=ReservationManagementSystem;Trusted_Connection=True;" providerName="System.Data.SqlClient" />
</connectionStrings>
Ejemplo para autenticación SQL Server (usuario y contraseña):
<connectionStrings>
   <add name="DefaultConnection" connectionString="Server=MI_SERVIDOR;Database=ReservationManagementSystem;User Id=mi_usuario;Password=mi_contraseña;" providerName="System.Data.SqlClient" />
</connectionStrings>
4. Instalar los Paquetes NuGet
El proyecto utiliza los siguientes paquetes NuGet:

Unity: Para inyección de dependencias.
Unity.Mvc5: Integración de Unity con ASP.NET MVC.
Dapper: Micro ORM para facilitar el acceso a la base de datos.
Para asegurarte de que estos paquetes estén instalados:

Abre la Consola del Administrador de Paquetes en Visual Studio.
Ejecuta los siguientes comandos:
Install-Package Unity
Install-Package Unity.Mvc5
Install-Package Dapper
5. Construir y Ejecutar el Proyecto
