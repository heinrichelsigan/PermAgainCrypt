https://cqrxs.eu/download/EU.CqrXs.SpoolTest/README.MD
EU.CqrXs.Spooler.exe spools a large amount of files from indir and en-/decrypt it to outdir
keys are given by keyfile (all domains of email domains in linux kernel).

EU.CqrXs.SpoolTest.exe -?
Usage:  EU.CqrXs.Spooler.exe
        -i  | --inDir={SpoolerInFilesDirectory}
        -o  | --outDir={SpoolerOutFilesDirectory}
		-D  | --Decrypt 	(decrypt instead of encrypt, by generating pipe from filename)
        -V  | --verbose 	(verbose output)
		-k  | --key={Key} 	(read from stdin, when parameter is missing to have real secret)
        -?  | --gethelp		(shows that help)

Example:
    
    EU.CqrXs.Spooler.exe -V -k=bar@ba.area23.at -i=H:\Spooler\InFiles\ -o=H:\Spooler\OutFiles\ 


