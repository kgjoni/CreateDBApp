
//
// Console app to create a database, e.g. BikeHike.
//
// KRISTI GJONI
// U. of Illinois, Chicago
// CS480, Summer 2018
// Project #1
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateDBApp
{
  class Program
  {

    static void Main(string[] args)
    {
      Console.WriteLine();
      Console.WriteLine("** Create Database Console App **");
      Console.WriteLine();

      string baseDatabaseName = "BikeHike";
      string sql;

            try
            {
                //
                // 1. Make a copy of empty MDF file to get us started:
                //
                Console.WriteLine("Copying empty database to {0}.mdf and {0}_log.ldf...", baseDatabaseName);

                CopyEmptyFile("__EmptyDB", baseDatabaseName);

                Console.WriteLine();

                //
                // 2. Now let's make sure we can connect to SQL Server on local machine:
                //
                DataAccessTier.Data data = new DataAccessTier.Data(baseDatabaseName + ".mdf");

                Console.Write("Testing access to database: ");

                if (data.TestConnection())
                    Console.WriteLine("success");
                else
                    Console.WriteLine("failure?!");

                Console.WriteLine();

                //
                // 3. Create tables by reading from .sql file and executing DDL queries:
                //
                Console.WriteLine("Creating tables by executing {0}.sql file...", baseDatabaseName);

                string[] lines = System.IO.File.ReadAllLines(baseDatabaseName + ".sql");

                sql = "";

                for (int i = 0; i < lines.Length; ++i)
                {
                    string next = lines[i];

                    if (next.Trim() == "")  // empty line, ignore...
                    {
                    }
                    else if (next.Contains(";"))  // we have found the end of the query:
                    {
                        sql = sql + next + System.Environment.NewLine;

                        Console.WriteLine("** Executing '{0}'...", sql.Substring(0, 32));

                        data.ExecuteActionQuery(sql);

                        sql = "";  // reset:
                    }
                    else  // add to existing query:
                    {
                        sql = sql + next + System.Environment.NewLine;
                    }
                }

                Console.WriteLine();

                //
                // 4. Insert data by parsing data from .csv files:
                //
                Console.WriteLine("Inserting data...");

                //
                // TODO...
                //
                Console.WriteLine("**TODO**");

                //  using  stmt  will  close  file  when  scope  is  exited:
                //
                

                using (var file = new System.IO.StreamReader("customers.csv"))
                {
                    while (!file.EndOfStream)
                    {
                        string line = file.ReadLine();
                        string[] values = line.Split(',');
                        int typeid = Convert.ToInt32(values[0]);
                        string firstname = values[1];
                        string lastname = values[2];
                        string email = values[3];



                        sql = string.Format(@" SET IDENTITY_INSERT Customer ON
                    Insert Into
                      Customer(CID,FirstName,LastName, Email)
                      Values({0},'{1}','{2}','{3}');
                    SET IDENTITY_INSERT Customer OFF",
                        typeid, firstname, lastname, email);

                        data.ExecuteActionQuery(sql);

                        //Console.WriteLine(sql);

                        //
                        // Done
                        //
                    }
                }

                using (var file = new System.IO.StreamReader("biketypes.csv"))
                {
                    while (!file.EndOfStream)
                    {
                        string line = file.ReadLine();
                        string[] values = line.Split(',');
                        int typeid = Convert.ToInt32(values[0]);
                        string description = values[1];
                        double priceperhour = Convert.ToDouble(values[2]);



                        sql = string.Format(@" SET IDENTITY_INSERT Byke_Type ON
                    Insert Into
                      Byke_Type(BTID,Description,PricePerHour)
                      Values({0},'{1}',{2});
                    SET IDENTITY_INSERT Byke_Type OFF",
                        typeid, description, priceperhour);

                        data.ExecuteActionQuery(sql);

                        //Console.WriteLine(sql);

                        //
                        // Done
                        //
                    }
                }

                using (var file = new System.IO.StreamReader("bikes.csv"))
                {
                    while (!file.EndOfStream)
                    {
                        string line = file.ReadLine();
                        string[] values = line.Split(',');
                        int typeid = Convert.ToInt32(values[0]);
                        int btid = Convert.ToInt32(values[1]);
                        int serviceyear = Convert.ToInt32(values[2]);



                        sql = string.Format(@" SET IDENTITY_INSERT Bike ON
                    Insert Into
                      Bike(BID,BTID,ServiceYear)
                      Values({0},{1},{2});
                    SET IDENTITY_INSERT Bike OFF",
                        typeid, btid, serviceyear);

                        data.ExecuteActionQuery(sql);

                        //Console.WriteLine(sql);

                        //
                        // Done
                        //
                    }
                }
            }

            catch (Exception ex)
      {
        Console.WriteLine("**Exception: '{0}'", ex.Message);
      }

      Console.WriteLine();
      Console.WriteLine("** Done **");
      Console.WriteLine();
    }//Main


    /// <summary>
    /// Makes a copy of an existing Microsoft SQL Server database file 
    /// and log file.  Throws an exception if an error occurs, otherwise
    /// returns normally upon successful copying.  Assumes files are in
    /// sub-folder bin\Debug or bin\Release --- i.e. same folder as .exe.
    /// </summary>
    /// <param name="basenameFrom">base file name to copy from</param>
    /// <param name="basenameTo">base file name to copy to</param>
    static void CopyEmptyFile(string basenameFrom, string basenameTo)
    {
      string from_file, to_file;

      //
      // copy .mdf:
      //
      from_file = basenameFrom + ".mdf";
      to_file = basenameTo + ".mdf";

      if (System.IO.File.Exists(to_file))
      {
        System.IO.File.Delete(to_file);
      }

      System.IO.File.Copy(from_file, to_file);

      // 
      // now copy .ldf:
      //
      from_file = basenameFrom + "_log.ldf";
      to_file = basenameTo + "_log.ldf";

      if (System.IO.File.Exists(to_file))
      {
        System.IO.File.Delete(to_file);
      }

      System.IO.File.Copy(from_file, to_file);
    }

  }//class
}//namespace

