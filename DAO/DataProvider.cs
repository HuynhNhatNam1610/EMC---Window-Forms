using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace EMC.DAO
{ 
    public class DataProvider
    {

        private string connectionSTR = ConfigurationManager.ConnectionStrings["MyDbConnection"].ConnectionString;
        private static DataProvider instance;
        public static DataProvider Instance
        {
            get { if (instance == null) instance = new DataProvider(); return DataProvider.instance; }
            private set { DataProvider.instance = value; }
        }

        public DataTable ExecuteProcedure(string procedureName, object[] parameter = null)
        {
            DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionSTR))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(procedureName, connection);
                command.CommandType = CommandType.StoredProcedure;

                if (parameter != null)
                {
                    // Nếu procedure có tham số thì truyền vào
                    for (int i = 0; i < parameter.Length; i++)
                    {
                        command.Parameters.AddWithValue($"@param{i + 1}", parameter[i]);
                    }
                }

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(data);

                connection.Close();
            }

            return data;
        }

        public DataTable ExecuteProcedureWithParameter(string procedureName, Dictionary<string, object> parameters = null)
        {
            DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionSTR))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(procedureName, connection);
                command.CommandType = CommandType.StoredProcedure;

                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value);
                    }
                }

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(data);

                connection.Close();
            }

            return data;
        }

        public int ExecuteNonQueryProcedure(string procedureName, Dictionary<string, object> parameters = null)
        {
            int data = 0;

            using (SqlConnection connection = new SqlConnection(connectionSTR))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(procedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                        }
                    }

                    data = command.ExecuteNonQuery();
                }

                connection.Close();
            }

            return data;
        }

        public Dictionary<string, object> ExecuteProcedureWithOutput(
            string procedureName,
            Dictionary<string, object> inputParams = null,
            Dictionary<string, SqlDbType> outputParams = null)
        {
            var outputs = new Dictionary<string, object>();

            using (SqlConnection connection = new SqlConnection(connectionSTR))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(procedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Input parameters
                    if (inputParams != null)
                    {
                        foreach (var param in inputParams)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                        }
                    }

                    // Output parameters
                    if (outputParams != null)
                    {
                        foreach (var param in outputParams)
                        {
                            command.Parameters.Add(param.Key, param.Value).Direction = ParameterDirection.Output;
                        }
                    }

                    command.ExecuteNonQuery();

                    // Lấy giá trị output trả về
                    if (outputParams != null)
                    {
                        foreach (var param in outputParams.Keys)
                        {
                            outputs[param] = command.Parameters[param].Value;
                        }
                    }
                }

                connection.Close();
            }

            return outputs;
        }

        public object ExecuteScalarProcWithTVP(
            string procedureName,
            Dictionary<string, object> parameters,
            string tvpParamName,
            string tvpTypeName,
            DataTable tvpTable
        )
        {
            using (SqlConnection connection = new SqlConnection(connectionSTR))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(procedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Thêm các tham số bình thường
                    if (parameters != null)
                    {
                        foreach (var kv in parameters)
                        {
                            command.Parameters.AddWithValue(kv.Key, kv.Value ?? DBNull.Value);
                        }
                    }

                    // Thêm tham số TVP
                    if (!string.IsNullOrEmpty(tvpParamName) && tvpTable != null)
                    {
                        var tvp = command.Parameters.AddWithValue(tvpParamName, tvpTable);
                        tvp.SqlDbType = SqlDbType.Structured;
                        tvp.TypeName = tvpTypeName; // ví dụ "dbo.ParamRow"
                    }

                    // Thực thi và trả về scalar (ví dụ sample_id mới)
                    object result = command.ExecuteScalar();
                    return result;
                }
            }
        }

        public DataSet ExecuteProcedureReturnDataSet(string procedureName, Dictionary<string, object> parameters = null)
        {
            DataSet ds = new DataSet();

            using (SqlConnection connection = new SqlConnection(connectionSTR))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(procedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                        }
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(ds);
                }

                connection.Close();
            }

            return ds;
        }


        public DataTable ExecuteQuery(string query, object[] parameter = null)
        {

            DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionSTR))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);

                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara) {
                        if (item.Contains('@')){
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    
                    }
                }

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(data);

                connection.Close();
            }

            return data;
        }

        public int ExecuteNonQuery(string query, object[] parameter = null)
        {

            int data = 0;

            using (SqlConnection connection = new SqlConnection(connectionSTR))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);

                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }

                    }
                }

                data = command.ExecuteNonQuery();

                connection.Close();
            }

            return data;
        }

        public object ExecuteScalar(string query, object[] parameter = null)
        {

            object data = 0;

            using (SqlConnection connection = new SqlConnection(connectionSTR))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);

                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }

                    }
                }

                data = command.ExecuteScalar();

                connection.Close();
            }

            return data;
        }
    }
}
