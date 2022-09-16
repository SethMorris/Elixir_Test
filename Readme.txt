

Created DB to store data.  Will need to update connection string in the Elixir_Test_DAL class.  
Normally this would be held outside in a config file
Could have also done in XML, Json or text file

Putting all the classes in a few files for simplicity, normally this would be split outside

The SQL functions would normally split out to a more generic class
Also I would use Stored proc to pull the data

more error logic should be added

Could have done with API call for BLL & DLL

