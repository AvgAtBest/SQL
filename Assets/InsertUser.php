<?php
	$server_name = "localhost";
	$server_user = "root";
	$server_password = "";
	$database_name = "SQueaLsystem";
	
	$username = $_POST["usernamePost"];
	$password = $_POST["passwordPost"];
	$email = $_POST["emailPost"];
		//establishes connection to database
	$conn = new mysqli($server_name, $server_user, $server_password, $database_name);
		//if it fails to gain a connection to the database
	if(!$conn)
	{
				//kill connection, return connection failed
		die("Conneciton Failed.". mysqli_connect_error());
	}
	//finds and grabs username from users table
	$finduser = "SELECT username FROM users";
	//sends a query, tries to find user
	$finduserresult = mysqli_query($conn, $finduser);
	$canmakeaccount = "";
	
	//if there are more then 0 rows
	if(mysqli_num_rows($finduserresult)>0)
	{
		//search through the rows
		while($row = mysqli_fetch_assoc($finduserresult))
		{
			//if there is a matching username
			if($row['username'] == $username)
			{
				//there is already a user by that name
				echo "User Already Exists";
			}
			else
			{
				//account can be made
				$canmakeaccount = "check email";
			}
		}
	}
	//if there is no result found in the database
	else if(mysqli_num_rows($finduserresult)<=0)
	{
		//creates a user. Inserts username data into username row, email data into email row 
		//and password data into password row
		$makeuser = "INSERT INTO users(username, email, password)
		VALUES('".$username."', '".$email."', '".$password."')";
		$makeuserresult = mysqli_query($conn, $makeuser);//sends query with makeuser data
		
		//if the data is valid
		if($makeuserresult)
		{
			//creates first ever user
			echo "Create First User";
		}
	}
	//if the account can be made and the result in the rows isnt 0
	if($canmakeaccount == "check email" && mysqli_num_rows($finduserresult)>0)
	{
		//grabs email from users table
		$checkemail = "SELECT email FROM users";
		$checkemailresult = mysqli_query($conn, $checkemail);
		if(mysqli_num_rows($checkemailresult)>0)
		{
			//checks and gets result in the row
			while($row = mysqli_fetch_assoc($checkemailresult))
			{
				//if the email in the row matches the email data
				if($row['email'] == $email)
				{
					//email already exists mate
					echo "Email Already exists";
				}
				else
				{
					//make a user, inputing username, email and password into table
					$makeuser = "INSERT INTO users(username, email, password)
					VALUES('".$username."', '".$email."', '".$password."')";
					$makeuserresult = mysqli_query($conn, $makeuser);
		
					if($makeuserresult)//if its a valid query
					{
						//create the user
						echo "Create User";
					}
				}
			}
			
		}
	}
?>