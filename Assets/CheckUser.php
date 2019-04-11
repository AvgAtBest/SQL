<?php
	$server_name = "localhost";
	$server_user = "root";
	$server_password = "";
	$database_name = "SQueaLsystem";
	

	$email = $_POST["emailPost"];
	//establishes connection to database
	$conn = new mysqli($server_name, $server_user, $server_password, $database_name);
	//if it fails to gain a connection to the database
	if(!$conn)
	{
		//connection failed
		die("Conneciton Failed.". mysqli_connect_error());
	}
	//selects user from users table where the email data sent from Login script matches a email in the table
	$checkuser = "SELECT username FROM users WHERE email = '".$email."'";
	//sends a query request, checks connection and user
	$checkuserresult = mysqli_query($conn, $checkuser);
	//if there is more then 0 users in the row
	if(mysqli_num_rows($checkuserresult) > 0)
	{
		//searchs through the row for user
		while($row = mysqli_fetch_assoc($checkuserresult))
		{
			//echo $row['username'];
			echo "user found";
			//user successfully found
		}
	}
	else
	{
		//no user found
		echo "no user";
	}


?>