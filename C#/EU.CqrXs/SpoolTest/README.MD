https://cqrxs.eu/download/EU.CqrXs.SpoolTest/README.MD

EU.CqrXs.SpoolTest.exe -?
Usage:  EU.CqrXs.SpoolTest.exe
        -i  | --inDir={SpoolerInFilesDirectory}
        -o  | --outDir={SpoolerOutFilesDirectory}
		-D  | --Decrypt ... see files in inDir as encrypted and decrypt
        -r  | --rotate frequences of algorithms to have more data for case study
		-k  | --keyFile={KeyFile} ... file with new key in each line		
        -?  | --gethelp

Example:
    
    EU.CqrXs.SpoolTest.exe -i=H:\Spooler\InFiles\ -o=H:\Spooler\OutFiles\ -r -k=H:\Spooler\keys.txt


