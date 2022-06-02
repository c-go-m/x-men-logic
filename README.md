# x-men-logic
Sistema que valida si un individuo es **Humano** o **Mutante**
## Descripción
Este sistema evalúa los AND de las personas y así poder determinar si un individuo es Mutante, además de esto también determina a cuantas
personas se les ha realizado la prueba, y de ellas cuantas son **Mutantes o Humanos** 
### ¿Como funciona?
* Para determinar si una persona es mutante se debe enviar su ADN de la siguiente manera:

```
Request
Method = POST
URL = https://x-men-function.azurewebsites.net/api/v1/mutant
BODY = { "dna": [ "TTTTATGCAT", "GCATGCATGC", "ATGCATGCAT", "GCATGCATGC", "ATGCATGCAT", "GCATGCATGC", "ATGCATGCAT", "GCATGCATGC", "ATGCTTGCAT", "GCATGCTTTT" ] }

Response
Mutant = Ok - 200
Human = Forbidden - 403
Error = Bad Request - 400 
```

* Para determinar cuántos individuos han sido evaluados y cuántos de ellos son **Mutantes o Humanos**:

```
Request
Method = GET
URL = https://x-men-function.azurewebsites.net/api/v1/stats

Response
Mutant = Ok - 200
Result = {
            "count_mutant_dna": n_Mutantes,
            "count_human_dna": n_Humanos,
            "ratio": Ratio
          }
Error = Bad Request - 400 
```

### ¿Como ejecutar el programa de manera local?

* #### Si se tiene Visual Studio 2019 o superior 
  * Solo es necesario abrir el proyecto y ejecutarlo como se muestra en las siguientes imágenes
    * Paso 1 
    * Paso 2
* #### Si se tiene Docker Desktop
  * Solo es necesario ejecutar los siguientes comandos  
    * Paso 1
    ```
      docker-compose up
    ```  
