<?php
	$server_name = "localhost";
	$server_user = "root";
	$server_password = "";
	$database_name = "SQueaLsystem";
	
	$username = $_POST["usernamePost"];
	$password = $_POST["passwordPost"];

	//establishes connection to database
	$conn = new mysqli($server_name, $server_user, $server_password, $database_name);
		//if it fails to gain a connection to the database
	if(!$conn)
	{
		//kill connection, return connection failed
		die("Conneciton Failed.". mysqli_connect_error());
	}
	//Locates and choses password from users table where username data from Login script matches database user
	$finduser = "SELECT password FROM users WHERE username = '".$username."' ";
	$finduserresult = mysqli_query($conn, $finduser);//finds user result, connects

	
	//Get the result and confirm login
	if(mysqli_num_rows($finduserresult)>0)
	{
		//search through the rows
		while($row = mysqli_fetch_assoc($finduserresult))
		{
			//if it finds a matching password from a selected username in the password row
			if($row['password'] == $password)
			{
				echo "Login success";
				//Login success
				
			}
			else
			{
				//wrong password dummy
				echo "password incorect";
				
			}
		}
	} else {
		echo "user not found";
		//couldnt find a matching username
	}


?>