<?php
require './db.php';

if ($conn->connect_error)
    die('Connection failed: ' . $conn->connect_error);

// User submited variables
$userID = $_POST["userID"];

// Prepared Statement
$sql = "SELECT id, itemID FROM usersitems where userID = ?";
$stmt = $conn->prepare($sql);
$stmt->bind_param("s", $userID);

$stmt->execute();
$result = $stmt->get_result();

if ($result->num_rows > 0) {
    // output data of each row
    $rows = array();
    while ($row = $result->fetch_assoc()) {
        $rows[] = $row;
    }
    // after the whole array is creatd 
    echo json_Encode($rows);
} else 
    echo '0';

$stmt->close();
$conn->close();
?>