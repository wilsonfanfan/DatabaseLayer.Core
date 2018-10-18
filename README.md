# DatabaseLayer.Core 
DatabaseLayer.Core  is a high-performance Micro-ORM that supports SQL Server This tutorial will introduce you to the basic operations of using DatabaseLayer.Core.

# Features
  - Providing a generation mapping file interface
  - The entity model inherits the PersistentObject interface and can directly call the entity: insert, delete, update, and Retrieve methods to manipulate the data.
  - Use the RetrieveCriteria query to return the DataTable, the IList result set, and support the cursor.
  - Support for multiple entities including operations in transactions

# Tutorial and Online entity generation tool 
See [Databaselayer](https://databaselayer.azurewebsites.net/)

# Entity Operation 
## Insert
```csharp
b_User b_User = new b_User();
b_User.UserID = "584280962@qq.com";
b_User.UserName = "wilson ";
b_User.Insert();
```
## Update
```csharp
b_User user = new b_UserManager().GetEntityObject("584280962@qq.com");
user.UserName = "wilson.fan";
user.Update();
```
## Delete
```csharp
b_User user = new b_UserManager().GetEntityObject("584280962@qq.com");
user.Delete();
```
## Retrieve
```csharp
b_User user=new b_User();
user.UserID="wilson.fan";
user.Retrieve();
```
## Transaction 
```csharp
Transaction ts = new Transaction();
try
{
    b_User user = new b_UserManager().GetEntityObject("584280962@qq.com");
    user.UserName = "wilson.fan";
    user.Update(ts);

    r_Score score = new r_ScoreManager().GetEntityObject(user.UserID);
    score.Score = 90;
    score.Update(ts);

    ts.CommitTransaction();
}
catch
{
    ts.RollbackTransaction();
}
```
## Execute a Command
```csharp
string strSql="delete from b_User where UserID='584280962@qq.com'";
int result = Query.ExecuteSQL(strSql,SystemEnvironment.Instance.DefaultDataSource);

string strSqlQuery = @"select * from b_User 
                  inner join r_Score 
                  on b_User.UserID=r_Score.UserID";
var dt = Query.ExecuteSQLQuery(strSqlQuery,SystemEnvironment.Instance.DefaultDataSource);
```


