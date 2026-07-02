\# Guía de Autenticación y Roles



Esta guía explica cómo probar el sistema de autenticación, autorización, roles, access token, refresh token y cierre de sesión del Sistema de Inventario.



\## 1. Roles del sistema



| Rol      | Permisos                                                                                                                                                    |

| -------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------- |

| Admin    | Administra usuarios y productos. Puede crear, consultar, actualizar y desactivar usuarios. También puede crear, consultar, actualizar y eliminar productos. |

| Employee | Puede consultar, crear y actualizar productos. No puede eliminar productos ni administrar usuarios.                                                         |

| Viewer   | Solo puede consultar productos. No puede crear, actualizar ni eliminar productos. Tampoco puede administrar usuarios.                                       |



\---



\## 2. Usuario administrador inicial



El sistema crea un usuario administrador inicial desde el seeder.



```json

{

&#x20; "email": "admin@empresa.com",

&#x20; "password": "Admin123\*",

&#x20; "rol": "Admin"

}

```



Este usuario permite iniciar sesión por primera vez y crear los demás usuarios del sistema.



\---



\## 3. Login



Endpoint:



```http

POST /api/v1/Auth/login

```



Body:



```json

{

&#x20; "email": "admin@empresa.com",

&#x20; "password": "Admin123\*"

}

```



Respuesta esperada:



```json

{

&#x20; "accessToken": "token\_jwt",

&#x20; "refreshToken": "token\_de\_renovacion",

&#x20; "expiration": "fecha\_expiracion",

&#x20; "nombre": "Administrador",

&#x20; "email": "admin@empresa.com",

&#x20; "rol": "Admin"

}

```



\---



\## 4. Autorizar Swagger



Después de iniciar sesión, copiar el valor de `accessToken`.



En Swagger, presionar el botón \*\*Authorize\*\* y pegar el token con este formato:



```txt

Bearer TU\_ACCESS\_TOKEN

```



Ejemplo:



```txt

Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...

```



Importante:



\* Debe llevar la palabra `Bearer`.

\* Debe llevar un espacio entre `Bearer` y el token.

\* No se debe pegar el `refreshToken` en el botón Authorize.



\---



\## 5. Refresh Token



Este endpoint sirve para renovar el `accessToken` cuando se vence.



Endpoint:



```http

POST /api/v1/Auth/refresh-token

```



Body:



```json

{

&#x20; "refreshToken": "TU\_REFRESH\_TOKEN"

}

```



Respuesta esperada:



```json

{

&#x20; "accessToken": "nuevo\_token\_jwt",

&#x20; "refreshToken": "nuevo\_token\_de\_renovacion",

&#x20; "expiration": "nueva\_fecha\_expiracion",

&#x20; "nombre": "Administrador",

&#x20; "email": "admin@empresa.com",

&#x20; "rol": "Admin"

}

```



Notas:



\* No se usa `Bearer` en este endpoint.

\* Se debe enviar el `refreshToken` en el body.

\* Al renovar, el refresh token anterior queda revocado.



\---



\## 6. Logout



Este endpoint permite cerrar sesión y revocar el refresh token.



Endpoint:



```http

POST /api/v1/Auth/logout

```



Body:



```json

{

&#x20; "refreshToken": "TU\_REFRESH\_TOKEN"

}

```



Respuesta esperada:



```json

{

&#x20; "message": "Sesión cerrada correctamente."

}

```



\---



\## 7. Crear usuarios



Solo el rol `Admin` puede crear usuarios.



Endpoint:



```http

POST /api/v1/User

```



Body para crear un usuario `Employee`:



```json

{

&#x20; "nombre": "Empleado Prueba",

&#x20; "email": "empleado1@gmail.com",

&#x20; "password": "Empleado123",

&#x20; "rol": "Employee"

}

```



Body para crear un usuario `Viewer`:



```json

{

&#x20; "nombre": "Consulta Prueba",

&#x20; "email": "viewer1@gmail.com",

&#x20; "password": "Viewer123",

&#x20; "rol": "Viewer"

}

```



Roles permitidos:



```txt

Admin

Employee

Viewer

```



\---



\## 8. Endpoints de usuarios



Todos los endpoints de usuarios requieren rol `Admin`.



```http

GET /api/v1/User

GET /api/v1/User/{id}

GET /api/v1/User/email/{email}

POST /api/v1/User

PUT /api/v1/User/{id}

DELETE /api/v1/User/{id}

```



El `DELETE` no elimina físicamente el usuario. Lo desactiva cambiando el campo:



```txt

Activo = false

```



\---



\## 9. Endpoints de productos por rol



| Endpoint                            | Admin | Employee | Viewer |

| ----------------------------------- | ----: | -------: | -----: |

| GET /api/v1/Product                 |    Sí |       Sí |     Sí |

| GET /api/v1/Product/{id}            |    Sí |       Sí |     Sí |

| GET /api/v1/Product/codigo/{codigo} |    Sí |       Sí |     Sí |

| POST /api/v1/Product                |    Sí |       Sí |     No |

| PUT /api/v1/Product/{id}            |    Sí |       Sí |     No |

| DELETE /api/v1/Product/{id}         |    Sí |       No |     No |



\---



\## 10. Pruebas recomendadas



\### Prueba con Admin



1\. Iniciar sesión con `admin@empresa.com`.

2\. Autorizar Swagger con el `accessToken`.

3\. Probar los siguientes endpoints:



```http

GET /api/v1/User

POST /api/v1/User

GET /api/v1/Product

POST /api/v1/Product

PUT /api/v1/Product/{id}

DELETE /api/v1/Product/{id}

```



Resultado esperado:



```txt

Todos deben funcionar correctamente.

```



\---



\### Prueba con Employee



1\. Crear usuario con rol `Employee`.

2\. Iniciar sesión con ese usuario.

3\. Autorizar Swagger con su `accessToken`.

4\. Probar los siguientes endpoints:



```http

GET /api/v1/Product

POST /api/v1/Product

PUT /api/v1/Product/{id}

DELETE /api/v1/Product/{id}

GET /api/v1/User

```



Resultado esperado:



```txt

GET Product      -> 200 OK

POST Product     -> 200 OK

PUT Product      -> 200 OK

DELETE Product   -> 403 Forbidden

GET User         -> 403 Forbidden

```



\---



\### Prueba con Viewer



1\. Crear usuario con rol `Viewer`.

2\. Iniciar sesión con ese usuario.

3\. Autorizar Swagger con su `accessToken`.

4\. Probar los siguientes endpoints:



```http

GET /api/v1/Product

POST /api/v1/Product

PUT /api/v1/Product/{id}

DELETE /api/v1/Product/{id}

GET /api/v1/User

```



Resultado esperado:



```txt

GET Product      -> 200 OK

POST Product     -> 403 Forbidden

PUT Product      -> 403 Forbidden

DELETE Product   -> 403 Forbidden

GET User         -> 403 Forbidden

```



\---



\## 11. Códigos de respuesta esperados



| Código                    | Significado                                             |

| ------------------------- | ------------------------------------------------------- |

| 200 OK                    | Solicitud correcta.                                     |

| 400 Bad Request           | Datos inválidos o incompletos.                          |

| 401 Unauthorized          | Token ausente, inválido o vencido.                      |

| 403 Forbidden             | Usuario autenticado, pero sin permisos para esa acción. |

| 404 Not Found             | Registro no encontrado.                                 |

| 500 Internal Server Error | Error interno del servidor.                             |



\---



\## 12. Flujo general de autenticación



```txt

Usuario inicia sesión

&#x20;       ↓

Sistema valida email y contraseña

&#x20;       ↓

Sistema genera access token y refresh token

&#x20;       ↓

Usuario usa access token para endpoints protegidos

&#x20;       ↓

Si el access token vence, usa refresh token

&#x20;       ↓

Si cierra sesión, se revoca el refresh token

```



Resumen del flujo:



1\. El usuario se autentica con email y contraseña.

2\. El sistema genera un `accessToken` para consumir endpoints protegidos.

3\. El sistema genera un `refreshToken` para renovar la sesión.

4\. Swagger usa el `accessToken` con formato `Bearer`.

5\. Los roles controlan qué puede hacer cada usuario.

6\. El cierre de sesión revoca el `refreshToken`.



