using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdoNetCorePractica.Helper;
using Microsoft.Data.SqlClient;

#region
//CREATE PROCEDURE SP_ALL_HOSPITALES
//AS
//	SELECT * FROM HOSPITAL
//GO



//CREATE PROCEDURE ObtenerInformacionHospital
//AS
//BEGIN
//    -- Combina los datos de la plantilla y los doctores con un UNION
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
//        h.HOSPITAL_COD = 22

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
//        h.HOSPITAL_COD = 22;
//END;
//GO


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
            string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS01;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=sa;Encrypt=True;Trust Server Certificate=True";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
        }

        public async Task<List<string>> GetHospitalesAsync()
        {
            string sql = "SP_ALL_HOSPITALES";
            this.com.CommandType = System.Data.CommandType.Text;
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

        public async Task<List<string>> GetEmpleadosHospital()
        {

        }
    }
}
