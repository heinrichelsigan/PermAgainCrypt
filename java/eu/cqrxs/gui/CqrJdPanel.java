/*
 *
 * @author           Heinrich Elsigan
 * @version          V 0.2
 * @since            JDK 8
 *
 * Coded 2021-2025 by
 * <a href="mailto:he@area23.at">Heinrich.Elsigan</a><a href="https://heinrichelsigan.area23.at">heinrichelsigan.area23.at</a>
 */ 
package eu.cqrxs.gui;

import eu.cqrxs.gui.CqrJdFrame;
import eu.cqrxs.gui.*;
import eu.cqrxs.gui.CqrJDialog;
import eu.cqrxs.gui.ImageViewer;
import eu.cqrxs.fw.util.Constants;
import eu.cqrxs.fw.crypt.hash.KeyHash;
import eu.cqrxs.fw.crypt.hash.*;
import eu.cqrxs.fw.zip.ZipType;
import eu.cqrxs.fw.zip.GZ;
import eu.cqrxs.fw.crypt.cipher.CipherEnum;
import eu.cqrxs.fw.crypt.cipher.*;
import eu.cqrxs.fw.crypt.encoding.*;

import java.nio.file.Files;
import java.nio.file.Path;
import java.awt.*;
import java.awt.event.*;
import java.io.File;
import java.io.InputStream;
import java.io.BufferedInputStream;
import java.lang.*;
import java.lang.IllegalStateException;
import java.net.http.*;
import java.net.*;
import java.time.Duration;
import java.util.HashSet;
import java.util.Set;
import javax.swing.*;
import javax.swing.filechooser.FileNameExtensionFilter;


public class CqrJdPanel extends JPanel {

	public static CqrJdPanel cqrJdPanel;
	protected static byte[] openFileBytes, saveFileBytes;
	URL fileInUrl, fileEnCryptedUrl, fileDeCryptedUrl, pipeUrl;
	/// at/net/res/img/crypt/file.png");/
	 		
	protected KeyHash keyHash = KeyHash.Hex;
	protected ZipType zipType = ZipType.None;
	protected CipherEnum cipherEnum = CipherEnum.Aes;
	protected String cipherString, encodeString;
	protected EncodeEnum encodeType = EncodeEnum.Base64;
		
	Font menuFont, cryptFont;  
	JLabel jLabel_fileIn = new JLabel(), jLabel_fileOut = new JLabel();		
	ImageViewer imPipe = new ImageViewer(), imInFile = new ImageViewer(), imOutFile  = new ImageViewer();

	public static void main(String args[]) {
		
		cqrJdPanel = new CqrJdPanel();
		cqrJdPanel.Init(cqrJdFrame);		
		cqrJdPanel.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
	}	

	public CqrJdFrame() {		
		Init(cqrJdFrame);		
	}

	public void Init() {
		setLayout(null);
		setSize(960,280);
		setResizable(false);
		
		cryptFont = new Font("Dialog", Font.PLAIN, 11);
		SymMouse aSymMouse = new SymMouse();		
						
		try {
			fileInUrl = new URL("https://area23.at/net/res/img/crypt/file.png");
			fileEnCryptedUrl = new URL("https://area23.at/net/res/img/crypt/encrypted.png");
			fileDeCryptedUrl = new URL("https://area23.at/net/res/img/crypt/decrypted.png");
		} catch (MalformedURLException mue) {
			mue.printStackTrace();
		}
		try {
			imInFile = new ImageViewer();
			imInFile.setImageURL(fileInUrl);
			imInFile.setBounds(8,8,60,60);	
			imInFile.addMouseListener(aSymMouse);			
			getContentPane().add(imInFile);
		} catch (Exception ex) {
			ex.printStackTrace();			
		}
		jLabel_fileIn = new JLabel();
		jLabel_fileIn.setFont(cryptFont);
		jLabel_fileIn.setBounds(8, 84, 120, 24);
		jLabel_fileIn.setText("[No input file loaded]");
		jLabel_fileIn.setFont(cryptFont);		
		getContentPane().add(jLabel_fileIn);

		try {
			imOutFile = new ImageViewer();
			imOutFile.setImageURL(fileInUrl);
			imOutFile.setBounds(892, 8, 60, 60);		
			imOutFile.addMouseListener(aSymMouse);
			getContentPane().add(imOutFile);
		} catch (Exception ex) {
			ex.printStackTrace();			
		}					
		
		jLabel_fileOut = new JLabel();
		jLabel_fileOut.setFont(cryptFont);
		jLabel_fileOut.setBounds(800, 84, 120, 24);
		jLabel_fileIn.setText("[No output file processed]");		
		getContentPane().add(jLabel_fileOut);				
									
		setVisible(true);
			
	}

	protected class SymMouse extends java.awt.event.MouseAdapter {
		public void mouseClicked(java.awt.event.MouseEvent event) {
			Object object = event.getSource();
			if (object == imInFile) {
				cipherString = cipherEnum.toString();
				String pipeText = jTextField_Pipe.getText();
				jTextField_Pipe.setText(pipeText + cipherString + ";");				
			} else if (object == imOutFile) {
				hashKey_action();				
			}
			else if (object == imPipe) {
			}
			else {
				
			}
		}
	}
	// class SymMouse extends java.awt.event.MouseAdapter 
	protected class SymMouse1 implements MouseListener {
		public void mousePressed(MouseEvent e) {
			Object object = e.getSource();
			if (object != null) {
			}		
		}
		public void mouseClicked(MouseEvent e) {
		}
		public void mouseEntered(MouseEvent e) {
		}
		public void mouseExited(MouseEvent e) {        
		}
		public void mouseReleased(MouseEvent e) {
		}
	}

	protected class SymAction implements ActionListener {
		public void actionPerformed(ActionEvent event) {
			Object object = event.getSource();

			if (object == menuMain_itemExit)
				exit_action(event);					
			else if (object == menuHelp_itemAbout)
				about_action(event);
			else if (object == menuHelp_itemHelp)
				help_action(event);
			else if (object == menuMain_itemHashKey)
				hashKey_action();
			else if (object == menuMain_itemOpen)
				open_action(event);
			else if (object == menuMain_itemSave)
				save_action(event);
			
			else if (object == jButton_encrypt || object == menuMain_itemEncrypt)
				encrypt_action(event);
			else if (object == jButton_decrypt || object == menuMain_itemDecrypt)
				decrypt_action(event);
			else if (object == jButton_setPipe || object == menuMain_itemSetPipe)
				setPipe_action(event);			
			else if (object == jButton_hashPipe || object == menuMain_itemHashPipe)
				hashPipe_action(event);
			else if (object == jButton_randomText || object == menuMain_itemRandomText)
				randomText_action(event);
			else if (object == jButton_resetForm || object == menuMain_itemReset)
				resetForm_action(event);
		}
	}

	protected void open_action(ActionEvent event) {                                 
		
		String initDirectory = (java.io.File.separatorChar == '/') ? System.getenv("HOME") : System.getenv("USERPROFILE");
		JFileChooser chooser = new JFileChooser();
		chooser.setCurrentDirectory(new File(initDirectory));
		// chooser.setFileFilter(new FileNameExtensionFilter("all files", "*.*"));
		int fileDialogResult = chooser.showOpenDialog(null);
		if (fileDialogResult == JFileChooser.CANCEL_OPTION || fileDialogResult == JFileChooser.ERROR_OPTION) {
			dbgMsg("open_action JFileChooser returned: " + fileDialogResult, 2, true);
			return;
		}
		
		File f = chooser.getSelectedFile();
		String filename = f.getAbsolutePath();
		
		try{
			openFileBytes = Files.readAllBytes(f.toPath());
			saveFileBytes = new byte[0];
			jButton_encrypt.requestFocus();
		} catch (Exception e){
			JOptionPane.showMessageDialog(null, e);
			e.printStackTrace();
		}                
    }   


	protected void save_action(ActionEvent event) {     
		
		String initDirectory = (java.io.File.separatorChar == '/') ? System.getenv("HOME") : System.getenv("USERPROFILE");
		JFileChooser chooser = new JFileChooser();
		chooser.setCurrentDirectory(new File(initDirectory));
		chooser.setFileFilter(new FileNameExtensionFilter("all files", "*.*"));
		int fileDialogResult = chooser.showSaveDialog(cqrJdFrame);
	
		File f = chooser.getSelectedFile();	
		
		Path filePath = f.toPath();
		try {
			if (saveFileBytes != null && saveFileBytes.length > 0) {
				Files.write(filePath, saveFileBytes);
			}
			else 
				throw new java.lang.IllegalStateException("saveFileBytes is null or len == 0");
		} catch (Exception ex) {
			JOptionPane.showMessageDialog(null, ex);
			ex.printStackTrace();
		}
			
	}
	

	protected void setPipe_action(ActionEvent event) {
		try {
			String key = jTextField_Key.getText().toString();
			String hashed = keyHash.hash(key);
			jTextField_Hash.setText(hashed);

			CipherPipe pipe = new CipherPipe(key, hashed, encodeType, zipType, keyHash);

			CipherEnum[] cipherEnums = pipe.getInPipe();
			String pipeSting = "";
			for (int ci = 0; ci < cipherEnums.length; ci++)
				pipeSting = pipeSting + cipherEnums[ci].getName() + ";";
			jTextField_Pipe.setText(pipeSting);
			
		} catch (Exception e) {
			e.printStackTrace();
		}
	}
	
	protected void hashKey_action() {
		String keyValue = "";
		try {
				keyValue = jTextField_Key.getText().toString();
		} catch (Exception exi) {
				keyValue = "zen@area23.at";
		}
		String hashed = "";
		try {
				hashed = keyHash.hash(keyValue);
				jTextField_Hash.setText(hashed);
		} catch (Exception exh) {
		}
	}
	
	protected void hashPipe_action(ActionEvent event) {
		// to do: code goes here.
		try {
			jTextAreaSource.setText("jButton_hashPipe_action");
		} catch (Exception e) {
			e.printStackTrace();
		}
	}
		
	protected void randomText_action(ActionEvent event) {
		String currentFortune = eu.cqrxs.fw.util.Fortune.getFortune();
		jTextAreaSource.setText(currentFortune);
	}
	
	protected void resetForm_action(ActionEvent event) {		
		try {
			jTextAreaSource.setText("");
			jTextAreaDestination.setText("");
			jTextField_Pipe.setText("");
			jTextField_Hash.setText("");
			jTextField_Key.setText("zen@area23.at");
			// TODO: reset JComboBoxes
		} catch (Exception e) {
			e.printStackTrace();
		}
	}
	
	protected void encrypt_action(ActionEvent event) {
		
		String plain = jTextAreaSource.getText();
		String key = jTextField_Key.getText();
		String hashed = keyHash.hash(key);
		jTextField_Hash.setText(hashed);
		String cipherPipeString = jTextField_Pipe.getText();
		String pipeString = "";
		CipherEnum[] ciphers = new CipherEnum[0];
		if (cipherPipeString.length() > 0) {
			ciphers = CipherEnum.parsePipeText(cipherPipeString);
		}
		CipherPipe pipe = new CipherPipe(ciphers, 8, encodeType, zipType, keyHash);

		String encrypted = "";
		CipherEnum[] cipherEnums = pipe.getInPipe();
		for (int ci = 0; ci < cipherEnums.length; ci++)
			pipeString = pipeString + cipherEnums[ci].getName() + ";";
        
		dbgMsg(String.format("PipeString: %s \nEncoding: %s Hashing: %s zipping; %s", 
		 		pipeString, encodeType.getName(), keyHash.getName(), zipType.getName()), 2, false);

		try {
			dbgMsg(String.format("pipe.encrypt with key=%s, hash=%s, \nencode=%s keyHash=%s, zip=%s", 
			 	key, hashed, encodeType.getName(), keyHash.getName(), zipType.getName()), 4, false);

			encrypted = pipe.encrpytTextGoRounds(plain, key, hashed, encodeType, zipType, keyHash);
			jTextAreaDestination.setText(encrypted);
		} catch (Exception ex) {
			ex.printStackTrace();
			jTextAreaDestination.setText(ex.toString());
		}
	}
	
	protected void decrypt_action(ActionEvent event) {

		String encrypted = jTextAreaSource.getText();
		String key = jTextField_Key.getText();
		String hashed = keyHash.hash(key);
		jTextField_Hash.setText(hashed);
		String cipherPipeString = jTextField_Pipe.getText();
		CipherEnum[] ciphers = new CipherEnum[0];
		if (cipherPipeString.length() > 0) {
			ciphers = CipherEnum.parsePipeText(cipherPipeString);
		}

		CipherPipe pipe = new CipherPipe(ciphers, 8, encodeType, zipType, keyHash);

		String plain = "";
		CipherEnum[] cipherEnums = pipe.getOutPipe();
		String pipeSting = "";
		for (int ci = 0; ci < cipherEnums.length; ci++)
			pipeSting = pipeSting + cipherEnums[ci].getName() + ";";
		
        dbgMsg(String.format("Out pipe: %s \nEncoding: %s Hashing: %s zipping; %s",
		        pipeSting, encodeType.getName(), keyHash.getName(), zipType.getName()), 1, true);

		String decrypted = "";
		try {
			decrypted = pipe.decryptTextRoundsGo(encrypted, key, hashed, encodeType, zipType, keyHash);
			jTextAreaDestination.setText(decrypted);

			dbgMsg(String.format("pipe.decrypt with key=%s, hash=%s, \nencode=%s keyHash=%s, zip=%s",
			        key, hashed, encodeType.getName(), keyHash.getName(), zipType.getName()), 4, true);

		} catch (Exception ex) {
			jTextAreaDestination.setText(ex.toString());
			ex.printStackTrace();
		}
	}
	
	
	protected void about_action(ActionEvent event) {
	
		jTextAreaSource.append("About menu clicked, event: " + event + "\n");
		
        try {
            if (new File("eu/cqrxs/gui/cqrxs-eu.jpg").isFile())
			    cqrJDialog = new CqrJDialog("eu/cqrxs/gui/cqrxs-eu.jpg");
            else if (new File("cqrxs-eu.jpg").isFile()) 
			    cqrJDialog = new CqrJDialog("cqrxs-eu.jpg");
            else
                cqrJDialog = new CqrJDialog();

			cqrJDialog.showDialog(cqrJdFrame);
			// cqrJDialog.mousePressed(
		} catch (Exception exIO) {
			exIO.printStackTrace();
		}
	}
	
	protected void help_action(ActionEvent event) {
	
		String os = System.getProperty("os.name").toLowerCase();
		Runtime rt = Runtime.getRuntime();
		String url = "https://io.cqrxs.eu/help";
		boolean success = false;
		
		if (Desktop.isDesktopSupported()) {
            Desktop desktop = Desktop.getDesktop();
            try {
                desktop.browse(new URI(url));
				success = true;            
            } catch (URISyntaxException e) {
				e.printStackTrace();
			} catch (Exception ex) {
                ex.printStackTrace();
			}
		}
		if (!success) {
			try {
				if (os.indexOf("win") >= 0)	
					rt.exec("rundll32 url.dll,FileProtocolHandler " + url);
				else if (os.indexOf("mac") >= 0) 
					rt.exec("open " + url);
				else // if (os.indexOf("x") >=0 || os.indexOf("bsd") >= 0)
					rt.exec("xdg-open "  + url);	
			} catch (Exception rtException) {
				rtException.printStackTrace();
			}
		}		
	}

	protected void exit_action(ActionEvent event) {
		// We don't log exit events ;)
		System.exit(0);
	}
	
	
    protected void dbgMsg(String s, int level, boolean ignoreDbg) {
		if (s != null && s.length() > 0 && (Constants.DEBUG || ignoreDbg)) {
            System.out.println(level + ": \t" + s);
        }
    }
	
	protected Image setJarIncludedImage(String imgstr) {
		Image img = null;
		try {
			InputStream is = getClass().getResourceAsStream(imgstr);
			BufferedInputStream bis = new BufferedInputStream(is);
			// a buffer large enough for our image can be byte[] byBuf = = new byte[is.available()];
			byte[] byBuf = new byte[10000];  // is.read(byBuf);  or something like that...
			int byteRead = bis.read(byBuf, 0, 10000);
			img = Toolkit.getDefaultToolkit().createImage(byBuf);
 	 	} catch(Exception e) {
			e.printStackTrace();
 		}
		return img;
	}
	
	protected void MakeWebRequest() {
		
		HttpClient client = HttpClient.newBuilder()         
         .connectTimeout(Duration.ofSeconds(10))
         .build(); 
		 
		String area23R = "https://cqrxs.eu/net/R.aspx";
		URI uri23 = URI.create(area23R);
		
		 //.version(HttpClient.Version.HTTP_2)
		 
		// HttpClient client = new HttpClient();
			// .uri(URI.create("https://area23.at/net/R.aspx"))					
			// .followRedirects(Redirect.NORMAL)
			// .version(Version.HTTP_1_1)			
			// .connectTimeout(Duration.ofSeconds(20))   			
			// .authenticator(Authenticator.getDefault())			
			// .build();
		
		// HttpRequest.newBuilder(new URI("https://area23.at/net/R.aspx"))
		
		HttpRequest request = HttpRequest.newBuilder()
			.uri(URI.create(area23R))	
			.GET()
			.build();
	
		HttpResponse<String> response;
   
		try {   
			response = client.send(request, HttpResponse.BodyHandlers.ofString());
					
			jTextAreaDestination.append("GET " + area23R + " status = " + response.statusCode() + "\n");
			jTextAreaDestination.append("Headers: " + response.headers().allValues("content-type"));
			jTextAreaDestination.append("Body: \n " + response.body());  
		} catch (Exception ioEx) {
			ioEx.printStackTrace();
			jTextAreaDestination.append("Exception: " + ioEx + "\n");		
		}
		
	}

}
