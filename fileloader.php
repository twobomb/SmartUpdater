<?php
try {	

//ini_set('error_reporting', E_ALL);
//ini_set("display_errors", 1);

//move_uploaded_file($_FILES["build_zip"]["tmp_name"],"tempbuild1.zip");
//die;



function endsWith($string, $endString){
    $len = mb_strlen($endString);
    if ($len == 0) 
        return true;
    return (mb_substr($string, -$len) === $endString);
}

function extractWithRusNames($zippath,$extractDir){	
	$zip = new ZipArchive;
	$zip->open($zippath);
	for($i = 0; $i < $zip->numFiles;$i++){
		$name = $zip->getNameIndex($i, ZipArchive::FL_ENC_RAW);
		$name = iconv('CP866','UTF-8',$name);	
		$dir = $name;
		if(!endsWith($name,"/"))
			$dir = dirname($name);
		if(!file_exists("$extractDir/$dir"))
			mkdir("$extractDir/$dir",0777,true);
		if($data = $zip->getFromIndex($i))
			file_put_contents("$extractDir/$name", $data);
	}
	$zip->close();
}
	
	if($_SERVER["REQUEST_METHOD"] != "POST"){
			http_response_code(403);
		exit;
	}
	
	if(!isset($_POST["type"])){
		echo "Не найден type";
		exit;
	}
	switch($_POST["type"]){		
		case "list"://заменить list.json
			if(!isset($_FILES["list_json"])){
				var_dump($_FILES);
				exit;
			}
			if(file_exists("list.json"))
				unlink("list.json");
			move_uploaded_file($_FILES["list_json"]["tmp_name"], "list.json");
		break;
		case "build"://новый билд
			if(!file_exists("list.json")){				
				echo "Не найден файл list.json!";
				exit;
			}
			if(!isset($_FILES["current_json"])){				
				echo "Не найден current.json!";
				exit;
			}		

			$guid = json_decode(file_get_contents($_FILES["current_json"]["tmp_name"]), true)["GUID"];
			$ServerPath = json_decode(file_get_contents($_FILES["current_json"]["tmp_name"]), true)["ServerPath"];
			if(empty($guid)){
				echo "Не найден guid";
				exit;
			}				
						
			if(!isset($_FILES["build_zip"])){				
				echo "Не найден build.zip!";
				exit;
			}
			$json_data = file_get_contents('list.json');
			$data = json_decode($json_data, true);
			
			foreach($data as $v)
				if($v["GUID"] == $guid){
					$pathToProgDir = PathCombine("../",$v["Path"]);
					if (!file_exists($pathToProgDir)) 
						mkdir($pathToProgDir, 0777, true);
					
					$pathToBuild = PathCombine($pathToProgDir,$ServerPath);
					if (file_exists($pathToBuild)) {
						echo "Папка с этим билдом уже существует!";
						exit;
					}
					mkdir($pathToBuild, 0777, true);
					
					extractWithRusNames($_FILES["build_zip"]["tmp_name"],$pathToBuild);
					
					$pathToCur = PathCombine($pathToProgDir,"current.json");
					if(file_exists($pathToCur))
						unlink($pathToCur);
					move_uploaded_file($_FILES["current_json"]["tmp_name"], $pathToCur);
					$pathToAll = PathCombine($pathToProgDir,"all.json");
					$c = [];
					if(file_exists($pathToAll)){
						$c = json_decode(file_get_contents($pathToAll), true);
						unlink($pathToAll);
					}
					array_push($c,json_decode(file_get_contents($pathToCur), true));					
					$myfile = fopen($pathToAll, "w");
					fwrite($myfile, json_encode($c,JSON_PRETTY_PRINT | JSON_UNESCAPED_UNICODE));
					fclose($myfile);					
										
				}
			
		break;
		default:		
			echo "Не известный type = $_POST[type]";
			exit;
	}
	echo "OK";
	
	
} catch (Exception $e) {
	http_response_code(500);
}



function PathCombine($one, $other, $normalize = true) {

    # normalize
    if($normalize) {
        $one = str_replace('/', DIRECTORY_SEPARATOR, $one);
        $one = str_replace('\\', DIRECTORY_SEPARATOR, $one);
        $other = str_replace('/', DIRECTORY_SEPARATOR, $other);
        $other = str_replace('\\', DIRECTORY_SEPARATOR, $other);
    }

    # remove leading/trailing dir separators
    if(!empty($one) && substr($one, -1)==DIRECTORY_SEPARATOR) $one = substr($one, 0, -1);
    if(!empty($other) && substr($other, 0, 1)==DIRECTORY_SEPARATOR) $other = substr($other, 1);

    # return combined path
    if(empty($one)) {
        return $other;
    } elseif(empty($other)) {
        return $one;
    } else {
        return $one.DIRECTORY_SEPARATOR.$other;
    }

}

