# eTutor - API BACKEND

Este es un proyecto creado para la materia de Proyecto Final de la carrera  de Ingeniera de Software del INTEC. Consiste en una aplicación para que los estudiantes puedan hacer solicitud de tutorás de manera fácil y eficiente utilizando sus telefonos celulares. Lo que busca 

## Instalación 
Para poder ejecutar el proyecto de backend de esta aplicación primero debe de cumplir con las siguientes dependencia en su maquina para poder ejecutar el mismo

### Dependencias: 
- SDK .net Core 2.2.402
- Visual Studio 2019
- MySql Server 8.0.15
- Windows 7 en adelante

### Instrucciones para ejecutarlo:
1. Debe de abrir la **soución** del proyecto llamada **eTutor.SOLUTION.sln** con **Visual Studio 2019**. 
2. Luego debe de compilar la solución completa
3. Ejecutar las migraciones (*Instrucciones para ejecutar migraciones*)
4. Ejecutar el proyecto *eTutor.ServerApi* 


### Instrucciones para ejecutar las migraciones de Base de Datos
1. Abrir el archivo **appsettings.json** y modificar la cadena de conexión **MainConncetion** para que esta se conecte a la base de datos local de la maquina
2. Ir al archivo **DefaultDbContextFactory.cs** en el proyecto **eTutor.Persistence** dentro de la solución y especificar que utilice la cadena **MainConnection** en el metodo ```public ETutorContext CreateDbContext(string[] args)```
3. Luego ir al archivo **Startup.cs** y hacer lo mismo con la siguiente linea ` services.AddDbContext<ETutorContext>(...)`
4. Proceder a abrir **Package Manager Console**
5. Elegir el proyecto **Etutor.Persistence** como *Default Project* en el Package Manager Console
6. Asegurarse de que **ETutor.ServerApi** esté elegido como el Startup Project.
7. Proceder a escribir el comando **`Update-Database`** en el *Package Manager Console*


## Colaboración
Para colaborar en este proyecto se debe de trabajar de la siguiente manera:
* Si va a elaborar una nueva característica, debe de crear un branch que contenga la siguente estructura al ser nombrado **feature/(DESCRIPCION_CARACTERISTICA)**
* Si va a hacer la corrección de algún error, debe de crear un branch con la siguiente estructura **bugfix/(CORRECCION)**

Al finalizar el desarrollo, debe de iniciar un **Pull Requst** para someter el código a revisión de mínimo otro miembro del equipo, para que los cambios puedan ser integrados con la rama principal del proyecto.

## Autores
  * Trabajo Inicial
    * Juan Daniel Ozuna
    * Miguel Gonzalez
    * Angel Ruiz
    * Laura Gomez
    * Jose Arias
    * Joswald Martinez

## License
### **MIT**