#! /bin/bash
if [ -d "$HOME/elasticIngestor/" ]; then sudo rm -d -R $HOME/elasticIngestor/; fi

sudo mkdir $HOME/elasticIngestor
sudo chmod -R 777 $HOME/elasticIngestor/

echo "Files present in $(HOME)/elasticIngestor/"
ls $HOME/elasticIngestor/

unzip PlayerDataGenerator.zip -d $HOME/elasticIngestor/

cd $HOME/elasticIngestor

./PlayerDataGenerator

