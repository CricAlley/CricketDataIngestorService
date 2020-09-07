#! /bin/bash
InputFolderPath="$1"
if [ -d "$HOME/$InputFolderPath/" ]; then sudo rm -d -R $HOME/$InputFolderPath/; fi

sudo mkdir -p $HOME/$InputFolderPath/
sudo chmod 777 $HOME/$InputFolderPath

sudo mkdir $HOME/$InputFolderPath/Temp
sudo chmod 777 $HOME/$InputFolderPath/Temp

echo "Files present in $HOME/$InputFolderPath/"
ls $HOME/$InputFolderPath/
echo "Files present in $HOME/$InputFolderPath/Temp"
ls $HOME/$InputFolderPath/Temp/

wget -P $HOME/$InputFolderPath/Temp https://cricsheet.org/downloads/odis_male.zip
unzip $HOME/$InputFolderPath/Temp/odis_male.zip -d $HOME/$InputFolderPath/odis_male

wget -P $HOME/$InputFolderPath/Temp https://cricsheet.org/downloads/t20s_male.zip
unzip $HOME/$InputFolderPath/Temp/t20s_male.zip -d $HOME/$InputFolderPath/t20s_male

wget -P $HOME/$InputFolderPath/Temp https://cricsheet.org/downloads/bbl_male.zip
unzip $HOME/$InputFolderPath/Temp/bbl_male.zip -d $HOME/$InputFolderPath/bbl_male

wget -P $HOME/$InputFolderPath/Temp https://cricsheet.org/downloads/cpl_male.zip
unzip $HOME/$InputFolderPath/Temp/cpl_male.zip -d $HOME/$InputFolderPath/cpl_male

wget -P $HOME/$InputFolderPath/Temp https://cricsheet.org/downloads/ipl_male.zip
unzip $HOME/$InputFolderPath/Temp/ipl_male.zip -d $HOME/$InputFolderPath/ipl_male

wget -P $HOME/$InputFolderPath/Temp https://cricsheet.org/downloads/ntb_male.zip
unzip $HOME/$InputFolderPath/Temp/ntb_male.zip -d $HOME/$InputFolderPath/ntb_male

rm -d -R $HOME/$InputFolderPath/Temp/
