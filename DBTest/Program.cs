using Dapper;
using Newtonsoft.Json;
using Npgsql;
using System.Data;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;

internal class Program
{
    private static void Main(string[] args)
    {
        
        string connStr = @"雲端連線字串";

        Console.WriteLine("請輸入執行動作\n 1.查詢 \n 2.更新 \n 3.新增 \n 4.刪除\n 6.條件查詢");
        string action = Console.ReadLine();
        using (var cn = new NpgsqlConnection(connStr))
        {
            try
            {
                if(cn.State == ConnectionState.Closed)
                    cn.Open();
                switch (action)
                {
                    case "1"://Query
                        SelectExample(cn);
                        break;
                    case "2"://Update
                        UpdateExample(cn);
                        break;
                    case "3"://Insert
                        Insert(cn);
                        break;
                    case "4"://Delete
                        DeleteExample(cn);
                        break;
                    case "6"://參數查詢
                        Console.WriteLine("請輸入customer_id");
                        string id = Console.ReadLine();
                        if(!string.IsNullOrEmpty(id))
                            SelectByParam(id,cn);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"錯誤訊息 : {ex}");            
            }
        }
    }
    /// <summary>
    /// Query
    /// </summary>
    /// <param name="cn"></param>
    /// <returns></returns>
    private static void SelectExample(NpgsqlConnection cn)
    {
        string sql = @"SELECT * FROM CUSTOMER WHERE customer_id = 2";
        var queryResult = cn.Query<dynamic>(sql).ToList();
        Console.WriteLine(JsonConvert.SerializeObject(queryResult));
    }
    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="cn"></param>
    private static void UpdateExample(NpgsqlConnection cn)
    {
        string sql = @"UPDATE CUSTOMER SET area = 'New Taipei City' WHERE customer_id = 1";
        var queryResult = cn.Query<dynamic>(sql).ToList();
        Console.WriteLine("成功");
    }
    /// <summary>
    /// 刪除
    /// </summary>
    /// <param name="cn"></param>
    private static void DeleteExample(NpgsqlConnection cn)
    {
        string sql = @"DELETE FROM CUSTOMER WHERE customer_id = 5";
        var queryResult = cn.Query<dynamic>(sql).ToList();
        Console.WriteLine("成功");
    }
    /// <summary>
    /// 參數查詢
    /// </summary>
    /// <param name="cn"></param>
    private static void SelectByParam(string id, NpgsqlConnection cn)
    {
        string sql = @"SELECT * FROM CUSTOMER WHERE customer_id = @id";
        var param = new { id = int.Parse(id) };
        var queryResult = cn.Query<dynamic>(sql, param).ToList();
        Console.WriteLine(JsonConvert.SerializeObject(queryResult));
    }
    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="cn"></param>
    private static void Insert(NpgsqlConnection cn)
    {
        string sql = @"Insert INTO customer
                        (
	                        customer_id,
	                        name,
	                        area,
	                        email
                        )
                        VALUES
                        (
	                        5,
	                        'Angel',
	                        'Taichung',
	                        't105368082@gmail.com'
                        )";
        cn.Query(sql);
        Console.WriteLine("成功");
    }

}