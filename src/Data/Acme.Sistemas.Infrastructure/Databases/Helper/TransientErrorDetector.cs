using MySqlConnector;

namespace Acme.Sistemas.Infrastructure.Databases.Helper;

public static class TransientErrorDetector
{
    private static readonly HashSet<int> TransientMySqlErrorCodes = new()
    {
        1040, 1042, 1043, 1053, 1077, 1078, 1079, 1080,
        1158, 1159, 1160, 1161, 1184, 1189, 1190, 1203,
        1205, 1213, 2002, 2003, 2006, 2013
    };

    public static bool IsTransient(Exception ex) => ex switch
    {
        MySqlException mySql => TransientMySqlErrorCodes.Contains(mySql.Number),
        TimeoutException => true,
        _ => false
    };
}
