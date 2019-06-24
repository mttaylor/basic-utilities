#!/bin/bash
apikey="AKCp5bAiutLX4zVrovGu8Hy5ZNop7F7j9HiqaTraNbVrRBQw4fKvAH3KxSBmqZZPT35a8rFhd"
user="opto22qa"

#snapshot url
art_snap_url="https://artifactory.opto22.com/artifactory/api/storage/deb-packages-snapshot-local/opto22/grv-epic-pr1/release"

#release url
art_release_url="https://artifactory.opto22.com/artifactory/api/storage/deb-packages-release-local/opto22/grv-epic-pr1/release"

#version folders
echo "What is the major version folder?"
read major

echo "What is the build number?"
read build

#main="1.0"
#build="1.0.1-b111"

#package parent folder name
pkgs=( "all" "cortexa9hf-vfp-neon" "cortexa9hf-vfp-neon-mx6qdl" "imx6qnxtio" "x86_64-nativesdk" )

#:functions for paths and writing to output
function writePkgName() {
	curl -sS -k --insecure -u $user:$apikey $1"/"$major"/"$2$3 | jq -r '.children[].uri' | sed "s/^/$3/" >> $4
}

function writeSnapPkg() {
	writePkgName $art_snap_url $build"/" $1 snapshot_output.txt
}

function writeReleasePkg() {
	writePkgName $art_release_url '' $1  release_output.txt
}


#writing output to files
function getDiff() {
	rm snapshot_output.txt release_output.txt
	for pkg in "${pkgs[@]}"; do
		writeSnapPkg $pkg
		writeReleasePkg $pkg
	done

	#Comm the two files for diff output
	comm -13 <(sort release_output.txt) <(sort snapshot_output.txt) > $build-diff-packages.txt
}

#Verify that diff looks correct
function copyArt() {

	#snapshot url for copy API
	art_snap_copy_url="https://artifactory.opto22.com/artifactory/api/copy/deb-packages-snapshot-local/opto22/grv-epic-pr1/release"

	#release url for copy API
	art_release_copy_url="/deb-packages-release-local/opto22/grv-epic-pr1/release"

	cat $build-diff-packages.txt
	echo ""
	read -p "Does this file look correct? If so type y for yes to contiune otherwise type no to exit " -r
	echo ""
	if [[ $REPLY =~ ^[Yy]$ ]]; then
		#Deploy each new package
		filename=$build"-diff-packages.txt"
		echo $filename
		while read -r line; do
			name=$line
			#encode url for +'s
			name=$(echo $name | sed 's/\+/%2B/g')
			echo $name
			curl -H "X-JFrog-Art-Api:XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX" --insecure -X POST $art_snap_copy_url"/"$major"/"$build"/"$name"?to="$art_release_copy_url"/"$major"/"$build"/"$name
		done < "$filename"

		#copy diff to build number
		curl -H "X-JFrog-Art-Api:XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX" --insecure -X PUT "https://artifactory.opto22.com/artifactory"$art_release_copy_url"/"$major"/"$build"/"$filename

		#get manifest files
		for pkg in "${pkgs[@]}"; do
			manifests=( "Packages" "Packages.gz" "Release" "Release.gpg" )
			for manifest in "${manifests[@]}"; do
				curl -H "X-JFrog-Art-Api:XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX" --insecure -X POST $art_snap_copy_url"/"$major"/"$build"/"$pkg"/"$manifest"?to="$art_release_copy_url"/"$major"/"$build"/"$pkg"/"$manifest
			done
		done
	fi
}

getDiff
copyArt

