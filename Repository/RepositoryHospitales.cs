using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using AdoNetCorePractica.Helper;
using AdoNetCorePractica.Model;
using AdoNetCorePractica.Models;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic.Logging;

#region

//1--Procedimiento para obtener todos los hospitales

//CREATE PROCEDURE SP_ALL_HOSPITALES
//( @namehospital nvarchar(50) )
//AS
//	SELECT * FROM HOSPITAL
//GO


//2--Procedimiento pra los empleados de un hospital (doctores + plantilla)

//ALTER PROCEDURE SP_ALL_EMP_HOSPITAL
//AS
//BEGIN
//    SELECT 
//        h.NOMBRE AS HOSPITAL_NOMBRE,
//        p.APELLIDO,
//        p.SALARIO,
//        p.FUNCION AS TRABAJO
//    FROM 
//        HOSPITAL h
//    INNER JOIN 
//        PLANTILLA p ON h.HOSPITAL_COD = p.HOSPITAL_COD
//    WHERE 
//        h.NOMBRE = @namehospital 

//    UNION

//    SELECT 
//        h.NOMBRE AS HOSPITAL_NOMBRE,
//        d.APELLIDO,
//        d.SALARIO,
//        d.ESPECIALIDAD AS TRABAJO
//    FROM 
//        HOSPITAL h
//    INNER JOIN 
//        DOCTOR d ON h.HOSPITAL_COD = d.HOSPITAL_COD
//    WHERE 
//        h.NOMBRE = @namehospital;
//END;
//GO

//3--Procedimiento para obtener la suma de los salarios de un hospital

//CREATE PROCEDURE SP_ALL_EMP_HOSPITAL_OUT
//   ( @namehospital nvarchar(50),
//     @suma INT OUT,
//     @media FLOAT OUT,
//     @personas INT OUT )
//AS
//BEGIN
//    DECLARE @totalSuma INT;
//DECLARE @totalMedia FLOAT;
//DECLARE @totalPersonas INT;

//--Obtener la información de la plantilla
//    SELECT 
//        p.SALARIO
//    INTO #PLANTILLA_SALARIOS
//    FROM 
//        HOSPITAL h
//    INNER JOIN 
//        PLANTILLA p ON h.HOSPITAL_COD = p.HOSPITAL_COD
//    WHERE 
//        h.NOMBRE = @namehospital;

//--Obtener la información de los doctores
//    SELECT 
//        d.SALARIO
//    INTO #DOCTORES_SALARIOS
//    FROM 
//        HOSPITAL h
//    INNER JOIN 
//        DOCTOR d ON h.HOSPITAL_COD = d.HOSPITAL_COD
//    WHERE 
//        h.NOMBRE = @namehospital;

//--Calcular la suma total de salarios, la media de salarios y el total de personas
//    SELECT 
//        @totalSuma = SUM(SALARIO),
//    @totalMedia = AVG(SALARIO),
//    @totalPersonas = COUNT(*)
//    FROM 
//        (SELECT SALARIO FROM #PLANTILLA_SALARIOS
//         UNION ALL
//         SELECT SALARIO FROM #DOCTORES_SALARIOS) AS TODOS;

//    -- Asignar los valores de salida
//    SET @suma = @totalSuma;
//SET @media = @totalMedia;
//SET @personas = @totalPersonas;

//--Limpiar las tablas temporales
//    DROP TABLE #PLANTILLA_SALARIOS;
//    DROP TABLE #DOCTORES_SALARIOS;
//END;
//GO



//DECLARE @suma INT, @media FLOAT, @personas INT;

//--Ejecutar el procedimiento almacenado para el hospital "La Paz"
//EXEC SP_ALL_EMP_HOSPITAL_OUT
//   @namehospital = 'La Paz',
//   @suma = @suma OUTPUT,
//   @media = @media OUTPUT,
//   @personas = @personas OUTPUT;

//--Ver los resultados
//SELECT @suma AS Suma_Salarios, @media AS Media_Salarios, @personas AS Total_Personas;

#endregion

namespace AdoNetCorePractica.Repository
{
    public class RepositoryHospitales
    {
        SqlConnection cn;
        SqlCommand com;
        SqlDataReader reader;

        public RepositoryHospitales()
        {
            string connectionString = HelperConfiguration.GetConnectionString();
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
        }

        public async Task<List<string>> GetHospitalesAsync()
        {
            string sql = "SP_ALL_HOSPITALES";
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;

            await this.cn.OpenAsync();
            this.reader = await this.com.ExecuteReaderAsync();

            List<string> hospitales = new List<string>();

            while (await this.reader.ReadAsync())
            {
                string nombre = this.reader["NOMBRE"].ToString();
                hospitales.Add(nombre);
            }

            await this.reader.CloseAsync();
            await this.cn.CloseAsync();
            return hospitales;
        }

        public async Task<List<string>> GetEmpleadosHospitalAsync(string namehospital)
        {
            string sql = "SP_ALL_EMP_HOSPITAL";

            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = sql;

            this.com.Parameters.AddWithValue("@namehospital", namehospital);

            await this.cn.OpenAsync();
            this.reader = await this.com.ExecuteReaderAsync();

            List<string> emp = new List<string>();

            while (await this.reader.ReadAsync())
            {
                string apellido = this.reader["APELLIDO"].ToString();
                string trabajo = this.reader["TRABAJO"].ToString();
                emp.Add(apellido + " - " + trabajo);
            }
            await this.reader.CloseAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();

            return emp;
        }

        public async Task<EmpHospInfo> GetEmpleadosHospital(string hospitalName)
        {
            string sql = "SP_ALL_EMP_HOSPITAL_OUT";
            this.com.Parameters.AddWithValue("@namehospital", hospitalName);

            // Parámetros de salida
            SqlParameter pamSuma = new SqlParameter();
            pamSuma.ParameterName = "@suma";
            pamSuma.Value = 0;
            pamSuma.Direction = ParameterDirection.Output;
            this.com.Parameters.Add(pamSuma);

            SqlParameter pamMedia = new SqlParameter();
            pamMedia.ParameterName = "@media";
            pamMedia.Value = 0;
            pamMedia.Direction = ParameterDirection.Output;
            this.com.Parameters.Add(pamMedia);

            SqlParameter pamPersonas = new SqlParameter();
            pamPersonas.ParameterName = "@personas";
            pamPersonas.Value = 0;
            pamPersonas.Direction = ParameterDirection.Output;
            this.com.Parameters.Add(pamPersonas);

            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = sql;

            await this.cn.OpenAsync();

            await this.com.ExecuteNonQueryAsync();

            EmpHospInfo model = new EmpHospInfo();
            List<string> apellidos = new List<string>();

            this.reader = await this.com.ExecuteReaderAsync();
            while (await this.reader.ReadAsync())
            {
                string apellido = this.reader["APELLIDO"].ToString();
                apellidos.Add(apellido);
            }

            await this.reader.CloseAsync();

            model.Apellidos = apellidos;
            model.SumaSalarial = int.Parse(pamSuma.Value.ToString());
            model.MediaSalarial = int.Parse(pamMedia.Value.ToString());
            model.Personas = int.Parse(pamPersonas.Value.ToString());

            await this.cn.CloseAsync();
            this.com.Parameters.Clear();

            return model;
        }


    }
}

