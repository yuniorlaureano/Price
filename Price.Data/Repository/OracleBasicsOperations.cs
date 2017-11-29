using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;



namespace DataAccess.Repository
{
   
    public class OracleBasicsOperations
    {
        private ConnectionManager connectionManager; //Nameja las cadenas de coneccion
        private OracleConnection oracleConnection; 
        private Schema schema; //Representa la cadena de coneccion

        public OracleBasicsOperations()
        {
            connectionManager = new ConnectionManager();
        }

        /// <summary>
        /// Abre la coneccion
        /// </summary>
        /// <param name="schema">Represanta la cadena de coneccion en el Web.config</param>
        public void OpenConnection(Schema schema)
        {
            try
            {
                if (this.oracleConnection == null)
                {
                    oracleConnection = new OracleConnection(connectionManager.SetConnectionString(schema));
                    this.schema = schema;
                }

                if (this.schema != schema)
                {
                    if (this.oracleConnection.State == ConnectionState.Open)
                    {
                        this.CloseConnection();
                    }

                    oracleConnection = new OracleConnection(connectionManager.SetConnectionString(schema));
                    this.schema = schema;
                }

                if (this.oracleConnection.State == ConnectionState.Closed)
                {
                    oracleConnection.Open();
                }
            }
            catch (OracleException excep)
            {
                throw excep;
            }
        }

        /// <summary>
        /// Cierra la conneccion
        /// </summary>
        public void CloseConnection()
        {
            try
            {
                if (this.oracleConnection != null && this.oracleConnection.State == ConnectionState.Open)
                {
                    this.oracleConnection.Close();
                }
            }
            catch (OracleException excep)
            {
                throw excep;
            }
        }

        /// <summary>
        /// Ejecuta una sentencia en la base de datos, y retorna un resultset con un dataset.
        /// </summary>
        /// <param name="query">sentencia sql</param>
        /// <param name="sqlParameters">arreglo con los parametros sql</param>
        /// <param name="commandType">Si es procedure y simple sentencia</param>
        /// <param name="schema">Cadena de conneccion</param>
        /// <returns>DataSet</returns>
        public DataSet ExecuteDataAdapter(string query, OracleParameter[] oracleParameters, CommandType commandType, Schema schema)
        {
            DataSet resultset = new DataSet();
            
            try
            {
                this.OpenConnection(schema);
                OracleCommand oracleCommand = new OracleCommand(query,this.oracleConnection);
                oracleCommand.CommandType = commandType;

                if (oracleParameters != null)
                {
                    oracleCommand.Parameters.AddRange(oracleParameters);
                }
                
                OracleDataAdapter oracledataAdapter = new OracleDataAdapter(oracleCommand);
                oracledataAdapter.Fill(resultset);
            }
            catch (OracleException excep)
            {                
                throw excep;
            }
            finally
            {
                this.CloseConnection();
            }

            return resultset;
        }

        /// <summary>
        /// Ejecuta una sentencia en la base de datos, y retorna un booleano. utilizado para cuando no es necesario tener un resultset desde la base de datos.
        /// </summary>
        /// <param name="query">sentencia sql</param>
        /// <param name="sqlParameters">arreglo con los parametros sql</param>
        /// <param name="commandType">Si es procedure y simple sentencia</param>
        /// <param name="schema">Cadena de conneccion</param>
        /// <returns>bool</returns>
        public bool ExecuteNonQuery(string query, OracleParameter[] oracleParameters, CommandType commandType,Schema schema)
        {
            bool resultset = false;

            try
            {
                this.OpenConnection(schema);
                OracleCommand oracleCommand = new OracleCommand(query,this.oracleConnection);
                oracleCommand.CommandType = commandType;

                if (oracleParameters != null)
                {
                    oracleCommand.Parameters.AddRange(oracleParameters);
                }

                resultset = oracleCommand.ExecuteNonQuery() > 0;
                
            }
            catch (OracleException excep)
            {
                throw excep;
            }

            return resultset;
        }

        /// <summary>
        /// Ejecuta una sentencia en la base de datos, y retorna un datareader. En caso de requerir mas performance.+
        /// </summary>
        /// <param name="query">sentencia sql</param>
        /// <param name="sqlParameters">arreglo con los parametros sql</param>
        /// <param name="commandType">Si es procedure y simple sentencia</param>
        /// <param name="schema">Cadena de conneccion</param>
        /// <returns>OracleDataReader</returns>
        public OracleDataReader ExecuteDataReader(string query, OracleParameter[] oracleParameters, CommandType commandType, Schema schema)
        {
            OracleDataReader reader;

            try
            {
                this.OpenConnection(schema);
                OracleCommand oracleCommand = new OracleCommand(query, this.oracleConnection);
                oracleCommand.CommandType = commandType;

                if (oracleParameters != null)
                {
                    oracleCommand.Parameters.AddRange(oracleParameters);
                }

                reader = oracleCommand.ExecuteReader();
            }
            catch (OracleException excep)
            {
                throw excep;
            }

            return reader;
        }
    }
}
