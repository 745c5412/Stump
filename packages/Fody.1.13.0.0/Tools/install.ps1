﻿param($installPath, $toolsPath, $package, $project)

function RemoveForceProjectLevelHack($project)
{
	$itemToRemove = $project.ProjectItems.Item("Fody_ToBeDeleted.txt")	
	$itemToRemove.Delete()	
}

function InjectTargets($installPath, $project)
{
	$targetsFile = [System.IO.Path]::Combine($installPath, 'Fody.targets')
 
    # Need to load MSBuild assembly if it's not loaded yet.
    Add-Type -AssemblyName 'Microsoft.Build, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
    # Grab the loaded MSBuild project for the project
    $msbuild = [Microsoft.Build.Evaluation.ProjectCollection]::GlobalProjectCollection.GetLoadedProjects($project.FullName) | Select-Object -First 1
 
	$importsToRemove = $msbuild.Xml.Imports | Where-Object { $_.Project.Endswith('Fody.targets') }
  
	# remove existing imports
	Foreach ($importToRemove in $importsToRemove) 
	{
		$msbuild.Xml.RemoveChild($importToRemove) | out-null
	}

    # Make the path to the targets file relative.
    $projectUri = new-object Uri('file://' + $project.FullName)
    $targetUri = new-object Uri('file://' + $targetsFile)
    $relativePath = $projectUri.MakeRelativeUri($targetUri).ToString().Replace([System.IO.Path]::AltDirectorySeparatorChar, [System.IO.Path]::DirectorySeparatorChar)
 
    # Add the import and save the project
	$importElement = $msbuild.Xml.AddImport($relativePath) 
	$importElement.Condition = "Exists('" + $relativePath + "')"
}

RemoveForceProjectLevelHack $project
InjectTargets $installPath $project

$project.Save()
