param($rootPath, $toolsPath, $package, $project)

function get-content-path($contentRoot) {
	$moniker = $project.Properties.Item("TargetFrameworkMoniker").Value
	$frameworkname = new-object System.Runtime.Versioning.FrameworkName($moniker)

	$id = $frameworkname.Identifier

	if($id -eq ".NETFramework") { $relative = "NET40" }
	elseif($id -eq "Silverlight" -and $frameworkname.Profile -eq "WindowsPhone") { $relative = "SL40-WP" }
	elseif($id -eq "Silverlight" ) { $relative = "SL40" }
 
	[System.IO.Path]::Combine($contentRoot, $relative)
}
 
$contentSource = get-content-path($rootPath + "\tools")
$defaultNamespace = $project.Properties.Item("DefaultNamespace").Value

ls $contentSource | foreach-object { 
	$content = [System.IO.File]::ReadAllText($_.FullName)
	$content = $content.Replace('$safeprojectname$', $defaultNamespace)
	$content | out-file -Encoding UTF8 $_.FullName
	$project.ProjectItems.AddFromFileCopy($_.FullName)
}

$project.DTE.ItemOperations.Navigate('http://caliburnmicro.codeplex.com/wikipage?title=Nuget')