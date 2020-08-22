<?php
require './db.php';

// Check connection
if ($conn->connect_error)
    die('Connection failed: ' . $conn->connect_error);

// variables submited by user
$loginUser = $_POST['loginUser'];
$loginPass = $_POST['loginPass'];

// Prepared Statement
$sql = "SELECT id, password FROM users where username = ?";
$stmt = $conn->prepare($sql);
$stmt->bind_param("s", $loginUser);

$stmt->execute();
$result = $stmt->get_result();

if ($result->num_rows > 0) {
    while ($row = $result->fetch_assoc()) {
        if ($row['password'] == $loginPass){
            echo $row['id'];
            // Get user's data here
            // Get player info
            // Modify player data(level, coin, etc.)
            // Update inventory
        }
        else
            echo 'Wrong Credentials';
    }
} else {
    echo 'Username does not exists';
}
$stmt->close();
$conn->close();
?>
