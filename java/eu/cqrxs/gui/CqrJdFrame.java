/*
	CqrJdFrame
	https://heinrichelsigan.area23.at
*/
package eu.cqrxs.gui;

import eu.cqrxs.gui.CqrJdFrame;
import eu.cqrxs.gui.*;
import eu.cqrxs.gui.CqrJDialog;

import java.awt.*;
import java.awt.event.*;
import java.io.File;
import java.io.InputStream;
import java.io.BufferedInputStream;
import java.lang.*;
import java.net.http.*;
import java.net.*;
import java.time.Duration;
import java.util.HashSet;
import java.util.Set;
import javax.swing.*;


public class CqrJdFrame extends JFrame {

	public static CqrJdFrame cqrJdFrame;
	URL keyUrl, hashUrl, addAlgoUrl, fileInUrl, fileOutUrl;
	/// at/net/res/img/crypt/file.png");/
	 
	
	JTextField jTextField_Key, jTextField_Hash, jTextField_Pipe;
	JComboBox jComboBox = new JComboBox(), jComboBox_Hash = new JComboBox(), jComboBox_Zip = new JComboBox(), jComboBox_Algo= new JComboBox(), jComboBox_Encoding = new JComboBox();
	
	JButton jButton = new JButton(), jButton_key = new JButton(), jButton_setPipe = new JButton(), jButton_hash = new JButton(), 
			jButton_hashPipe = new JButton(), jButton_addAlgo = new JButton(), 
			jButton_encrypt = new JButton(), jButton_decrypt = new JButton(), jButton_randomText = new JButton(), jButton_resetForm = new JButton();
	JPanel jPanelCenter = new JPanel();
	JTextArea jTextAreaSource = new JTextArea(), jTextAreaDestination = new JTextArea();
	CqrJDialog cqrJDialog;
	ImageViewer imKey = new ImageViewer(), imgHash = new ImageViewer(), imgAddAlgo = new ImageViewer(),  
				imgInFile = new ImageViewer(),  imgOutFile = new ImageViewer();
	
	JMenuBar jMenuBar = new JMenuBar();
	// JMenuBar jMenuBar = new JMenuBar();
	JMenu menuMain, menuZip, menuEncoding, menuHash, menuOptions, menuHelp = new JMenu();
	
	JMenuItem menuMain_itemOpen, menuMain_itemSave, 
				menuMain_itemSetPipe, menuMain_itemHashKey, menuMain_itemHashPipe, 
				menuMain_itemEncrypt, menuMain_itemDecrypt, menuMain_itemRandomText, menuMain_itemReset,
				menuMain_itemExit = new JMenuItem();

	JMenuItem menuZip_item7z, menuZip_itemGz, menuZip_itemBz, menuZip_itemZip, menuZip_itemNone;
	
	JMenuItem menuEncoding_itemNone, menuEncoding_itemBase16, menuEncoding_itemHex16, menuEncoding_itemUu, menuEncoding_itemXx, menuEncoding_itemBase64;
	
	JMenuItem menuHash_Ascon256, menuHash_Blake2xs, menuHash_BCrypt, menuHash_CShake, menuHash_MD5, menuHash_Hex, menuHash_OpenBSBCrypt,
				menuHash_RipeMD256, menuHash_Sha1, menuHash_Sha256, menuHash_Sha512, menuHash_SCrypt, menuHash_Whirlpool, menuHash_Xoodyak;

	JMenu menuOptions_menuWarnings, menuOptions_verifyEncryption, menuOptions_menuFileSettings;
	JMenuItem menuOptions_menuWarnings_itemWarnOnEmptyPipe, menuOptions_menuWarnings_itemWarnOnDoubleZipping;
	
	JMenuItem menuHelp_itemAbout = new JMenuItem(), menuHelp_itemHelp = new JMenuItem();		
	//}}

	public static void main(String args[]) {
		
		cqrJdFrame = new CqrJdFrame();
		cqrJdFrame.setLayout(null);
		cqrJdFrame.setSize(1024,768);
		cqrJdFrame.Init(cqrJdFrame);
		cqrJdFrame.setVisible(true);
		cqrJdFrame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
	}
	

	public CqrJdFrame() {

	}


	public void AddMenus(JMenuBar jBar) {
				
		/* Menu Main */		
		menuMain = new JMenu();
		menuMain.setText("Main");
		menuMain.setActionCommand("Main");
		menuMain.setFont(new Font("Dialog", Font.PLAIN, 12));
		menuMain.setMnemonic((int)'M');
		jBar.add(menuMain);
		
		menuMain_itemOpen = new JMenuItem();
		menuMain_itemOpen.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuMain_itemOpen.setText("Open...");
		menuMain_itemOpen.setActionCommand("Open...");
		menuMain_itemOpen.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_O, Event.CTRL_MASK));
		menuMain_itemOpen.setMnemonic((int)'O');
		menuMain.add(menuMain_itemOpen);
		
		menuMain_itemSave = new JMenuItem();
		menuMain_itemSave.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuMain_itemSave.setText("Save");
		menuMain_itemSave.setActionCommand("Save");
		menuMain_itemSave.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_S, Event.CTRL_MASK));
		menuMain_itemSave.setMnemonic((int)'S');
		menuMain.add(menuMain_itemSave);

		menuMain_itemSetPipe = new JMenuItem();
		menuMain_itemSetPipe.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuMain_itemSetPipe.setText("Set Pipe");
		menuMain_itemSetPipe.setActionCommand("SetPipe");
		menuMain.add(menuMain_itemSetPipe);

		menuMain_itemHashKey = new JMenuItem();
		menuMain_itemHashKey.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuMain_itemHashKey.setText("Hash Key");
		menuMain_itemHashKey.setActionCommand("HashKey");
		menuMain.add(menuMain_itemHashKey);

		menuMain_itemHashPipe = new JMenuItem();
		menuMain_itemHashPipe.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuMain_itemHashPipe.setText("Hash Pipe");
		menuMain_itemHashPipe.setActionCommand("HashPipe");
		menuMain.add(menuMain_itemHashPipe);

		menuMain_itemEncrypt = new JMenuItem();
		menuMain_itemEncrypt.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuMain_itemEncrypt.setText("Encrypt");
		menuMain_itemEncrypt.setActionCommand("Encrypt");
		menuMain.add(menuMain_itemEncrypt);

		menuMain_itemDecrypt = new JMenuItem();
		menuMain_itemDecrypt.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuMain_itemDecrypt.setText("Decrypt");
		menuMain_itemDecrypt.setActionCommand("Decrypt");
		menuMain.add(menuMain_itemDecrypt);

		menuMain_itemRandomText = new JMenuItem();
		menuMain_itemRandomText.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuMain_itemRandomText.setText("Random Text");
		menuMain_itemRandomText.setActionCommand("RandomText");
		menuMain.add(menuMain_itemRandomText);
		
		menuMain_itemReset = new JMenuItem();
		menuMain_itemReset.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuMain_itemReset.setText("Reset");
		menuMain_itemReset.setActionCommand("Reset");
		menuMain.add(menuMain_itemReset);

		menuMain_itemExit.setText("Exit");
		menuMain_itemExit.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_X, Event.ALT_MASK));
		menuMain_itemExit.setActionCommand("Exit");
		menuMain_itemExit.setMnemonic((int)'X');
		menuMain.add(menuMain_itemExit);
		
		/* Menu Compression */
		menuZip =  new JMenu();
		menuZip.setText("Compress");
		menuZip.setActionCommand("compress");
		jBar.add(menuZip);
				
		menuZip_itemNone = new JMenuItem();
		menuZip_itemNone.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuZip_itemNone.setText("None");
		menuZip_itemNone.setActionCommand("None");
		menuZip_itemNone.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_L, Event.CTRL_MASK));
		menuZip_itemNone.setMnemonic((int)'N');
		menuZip.add(menuZip_itemNone);
		
		menuZip_itemGz = new JMenuItem();
		menuZip_itemGz.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuZip_itemGz.setText("Gzip");
		menuZip_itemGz.setActionCommand("Gzip");
		menuZip_itemGz.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_L, Event.CTRL_MASK));
		menuZip_itemGz.setMnemonic((int)'G');
		menuZip.add(menuZip_itemGz);
		
		menuZip_itemBz = new JMenuItem();
		menuZip_itemBz.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuZip_itemBz.setText("Bzip"); 
		menuZip_itemBz.setActionCommand("Bzip");
		menuZip_itemBz.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_L, Event.CTRL_MASK));
		menuZip_itemBz.setMnemonic((int)'B');
		menuZip.add(menuZip_itemBz);
				
		menuZip_itemZip = new JMenuItem();
		menuZip_itemZip.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuZip_itemZip.setText("Zip");
		menuZip_itemZip.setActionCommand("Zip");
		menuZip_itemZip.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_L, Event.CTRL_MASK));
		menuZip_itemZip.setMnemonic((int)'Z');
		menuZip.add(menuZip_itemZip);		
		
		menuZip_item7z = new JMenuItem();
		menuZip_item7z.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuZip_item7z.setText("7z");
		menuZip_item7z.setActionCommand("7z");
		menuZip_item7z.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_L, Event.CTRL_MASK));
		menuZip_item7z.setEnabled(false);
		menuZip_item7z.setMnemonic((int)'7');
		menuZip.add(menuZip_item7z);
		
		menuEncoding = new JMenu();
		menuEncoding.setText("Encoding");
		menuEncoding.setActionCommand("Encoding");
		jBar.add(menuEncoding);
		
		menuEncoding_itemNone = new JMenuItem();
		menuEncoding_itemNone.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuEncoding_itemNone.setText("None");
		menuEncoding_itemNone.setActionCommand("None");
		// menuEncoding_itemNone.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_L, Event.CTRL_MASK));
		// menuEncoding_itemNone.setMnemonic((int)'L');
		menuEncoding.add(menuEncoding_itemNone);
		
		menuEncoding_itemBase16 = new JMenuItem();
		menuEncoding_itemBase16.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuEncoding_itemBase16.setText("Base16");
		menuEncoding_itemBase16.setActionCommand("Base16");
		// menuEncoding_itemBase16.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_L, Event.CTRL_MASK));
		// menuEncoding_itemBase16.setMnemonic((int)'L');
		menuEncoding.add(menuEncoding_itemBase16);
		
		menuEncoding_itemHex16 = new JMenuItem();
		menuEncoding_itemHex16.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuEncoding_itemHex16.setText("Hex16");
		menuEncoding_itemHex16.setActionCommand("Hex16");
		// menuEncoding_itemHex16.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_L, Event.CTRL_MASK));
		// menuEncoding_itemHex16.setMnemonic((int)'L');
		menuEncoding.add(menuEncoding_itemHex16);
		
		menuEncoding_itemUu = new JMenuItem();
		menuEncoding_itemUu.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuEncoding_itemUu.setText("Uu");
		menuEncoding_itemUu.setActionCommand("Uu");
		// menuEncoding_itemUu.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_L, Event.CTRL_MASK));
		// menuEncoding_itemUu.setMnemonic((int)'L');
		menuEncoding.add(menuEncoding_itemUu);
		
		
		menuEncoding_itemXx = new JMenuItem();
		menuEncoding_itemXx.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuEncoding_itemXx.setText("Uu");
		menuEncoding_itemXx.setActionCommand("Uu");
		// menuEncoding_itemXx.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_L, Event.CTRL_MASK));
		// menuEncoding_itemXx.setMnemonic((int)'L');
		menuEncoding.add(menuEncoding_itemXx);
			
			
		menuEncoding_itemBase64 = new JMenuItem();
		menuEncoding_itemBase64.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuEncoding_itemBase64.setText("Base64");
		menuEncoding_itemBase64.setActionCommand("Base64");
		// menuEncoding_itemBase64.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_L, Event.CTRL_MASK));
		// menuEncoding_itemBase64.setMnemonic((int)'L');
		menuEncoding.add(menuEncoding_itemBase64);
		
		
		menuHash = new JMenu();
		menuHash.setText("Hash");
		menuHash.setActionCommand("Hash");
		jBar.add(menuHash);
		
		menuHash_Ascon256 = new JMenuItem();
		menuHash_Ascon256.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuHash_Ascon256.setText("Ascon256");
		menuHash_Ascon256.setActionCommand("Ascon256");
		menuHash.add(menuHash_Ascon256);
		
		menuHash_Blake2xs = new JMenuItem();
		menuHash_Blake2xs.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuHash_Blake2xs.setText("Blake2xs");
		menuHash_Blake2xs.setActionCommand("Blake2xs");
		menuHash.add(menuHash_Blake2xs);
		
		menuHash_BCrypt = new JMenuItem();
		menuHash_BCrypt.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuHash_BCrypt.setText("BCrypt");
		menuHash_BCrypt.setActionCommand("BCrypt");
		menuHash.add(menuHash_BCrypt);
		
		menuHash_CShake = new JMenuItem();
		menuHash_CShake.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuHash_CShake.setText("CShake");
		menuHash_CShake.setActionCommand("CShake");
		menuHash.add(menuHash_CShake);
		
		menuHash_BCrypt = new JMenuItem();
		menuHash_BCrypt.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuHash_BCrypt.setText("BCrypt");
		menuHash_BCrypt.setActionCommand("BCrypt");
		menuHash.add(menuHash_BCrypt);
		
		menuHash_MD5 = new JMenuItem();
		menuHash_MD5.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuHash_MD5.setText("MD5");
		menuHash_MD5.setActionCommand("MD5");
		menuHash.add(menuHash_MD5);
		
		menuHash_Hex = new JMenuItem();
		menuHash_Hex.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuHash_Hex.setText("Hex");
		menuHash_Hex.setActionCommand("Hex");
		menuHash.add(menuHash_Hex);
		
		menuHash_OpenBSBCrypt = new JMenuItem();
		menuHash_OpenBSBCrypt.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuHash_OpenBSBCrypt.setText("OpenBSBCrypt");
		menuHash_OpenBSBCrypt.setActionCommand("OpenBSBCrypt");
		menuHash.add(menuHash_OpenBSBCrypt);
		
		menuHash_RipeMD256 = new JMenuItem();
		menuHash_RipeMD256.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuHash_RipeMD256.setText("RipeMD256");
		menuHash_RipeMD256.setActionCommand("RipeMD256");
		menuHash.add(menuHash_RipeMD256);
		
		menuHash_Sha1 = new JMenuItem();
		menuHash_Sha1.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuHash_Sha1.setText("Sha1");
		menuHash_Sha1.setActionCommand("Sha1");
		menuHash.add(menuHash_Sha1);		
		
		menuHash_Sha256 = new JMenuItem();
		menuHash_Sha256.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuHash_Sha256.setText("Sha256");
		menuHash_Sha256.setActionCommand("Sha256");
		menuHash.add(menuHash_Sha256);
		
		menuHash_Sha512 = new JMenuItem();
		menuHash_Sha512.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuHash_Sha512.setText("Sha512");
		menuHash_Sha512.setActionCommand("Sha512");
		menuHash.add(menuHash_Sha512);
		
		menuHash_SCrypt = new JMenuItem();
		menuHash_SCrypt.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuHash_SCrypt.setText("SCrypt");
		menuHash_SCrypt.setActionCommand("SCrypt");
		menuHash.add(menuHash_SCrypt);
				
		menuHash_Whirlpool = new JMenuItem();
		menuHash_Whirlpool.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuHash_Whirlpool.setText("Whirlpool");
		menuHash_Whirlpool.setActionCommand("Whirlpool");
		menuHash.add(menuHash_Whirlpool);
		
		menuHash_Xoodyak = new JMenuItem();
		menuHash_Xoodyak.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuHash_Xoodyak.setText("Xoodyak");
		menuHash_Xoodyak.setActionCommand("Xoodyak");
		menuHash.add(menuHash_Whirlpool);
		
		menuOptions = new JMenu();
		menuOptions.setText("Options");
		menuOptions.setActionCommand("Options");
		jBar.add(menuOptions);
		
		menuOptions_menuWarnings = new JMenu();
		menuOptions_menuWarnings.setText("Warnings");
		menuOptions_menuWarnings.setActionCommand("Warnings");
		menuOptions.add(menuOptions_menuWarnings);
		
		menuOptions_menuWarnings_itemWarnOnEmptyPipe = new JMenuItem();
		menuOptions_menuWarnings_itemWarnOnEmptyPipe.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuOptions_menuWarnings_itemWarnOnEmptyPipe.setText("Warn on empty pipe");
		menuOptions_menuWarnings_itemWarnOnEmptyPipe.setActionCommand("WarnOnEmptyPipe");
		menuOptions_menuWarnings.add(menuOptions_menuWarnings_itemWarnOnEmptyPipe);
				
		menuOptions_menuWarnings_itemWarnOnDoubleZipping = new JMenuItem();
		menuOptions_menuWarnings_itemWarnOnDoubleZipping.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuOptions_menuWarnings_itemWarnOnDoubleZipping.setText("Warn on double zipping");
		menuOptions_menuWarnings_itemWarnOnDoubleZipping.setActionCommand("WarnOnDoubleZipping");
		menuOptions_menuWarnings.add(menuOptions_menuWarnings_itemWarnOnDoubleZipping);
		
		menuHelp = new JMenu();
		menuHelp.setText("?");
		menuHelp.setActionCommand("?");
		menuHelp.setMnemonic((int)'?');		
		jBar.add(menuHelp);
		
		menuHelp_itemAbout = new JMenuItem();
		menuHelp_itemAbout.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuHelp_itemAbout.setText("About...");
		menuHelp_itemAbout.setActionCommand("About");
		menuHelp_itemAbout.setMnemonic((int)'A');
		menuHelp.add(menuHelp_itemAbout);
		
		menuHelp_itemHelp = new JMenuItem();
		menuHelp_itemHelp.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuHelp_itemHelp.setText("Help");
		menuHelp_itemHelp.setActionCommand("Help");
		menuHelp_itemHelp.setMnemonic((int)'H');
		menuHelp.add(menuHelp_itemHelp);
		
	}
	

	public void Init(JFrame jf) {
		// symantec.itools.lang.Context.setApplet(this);
		
		// getRootPane().putClientProperty("defeatSystemEventQueueCheck", Boolean.TRUE);
		try {
			keyUrl = new URL("https://area23.at/net/res/img/symbol/key_ring.gif");
			hashUrl = new URL("https://area23.at/net/res/img/crypt/a_hash.png");
			addAlgoUrl = new URL("https://area23.at/net/res/img/crypt/AddAesArrowHover.gif");
			fileInUrl = new URL("https://area23.at/net/res/img/crypt/file.png");
		} catch (MalformedURLException mue) {
			mue.printStackTrace();
		}

		jf.setLayout(null);
		jf.setSize(1024, 768);
		jf.setResizable(false);
		
		jMenuBar = new JMenuBar();
		AddMenus(jMenuBar);
		
		// jMenuBar.setBounds(0, 0, 480, 24);
		
		// jMenuBar.setSize(480,24);
		jf.setJMenuBar(jMenuBar);
		// jf.add(jMenuBar);
		// jMenuBar.move(0,  0);
		
		jComboBox.setBounds(48, 36, 640, 24);
		jf.getContentPane().add(jComboBox);
		
		jPanelCenter.setBounds(48, 72, 640, 400);
		jPanelCenter.setLayout(new GridLayout(1, 2));
		jPanelCenter.setBackground(Color.BLACK);  
		jPanelCenter.add(jTextAreaSource);
		jTextAreaSource.setBounds(1,1,632,196);
		jTextAreaSource.setBackground(Color.GRAY);  
		jTextAreaSource.append("jMenuBar.getUI() == " + jMenuBar.getUI() + "\n");		
		jPanelCenter.add(jTextAreaDestination);
		jTextAreaDestination.setBounds(1,240,632,196);
		jTextAreaDestination.setBackground(Color.YELLOW);  
		
		jf.getContentPane().add(jPanelCenter);
		
		jButton.setText("jbutton");
		jf.getContentPane().add(jButton);
		jButton.setBounds(24,600,76,48);
		jButton.setActionCommand("jbutton");
		
		
		
		jf.setVisible(true);
		//}}
	
		//{{REGISTER_LISTENERS
		SymAction lSymAction = new SymAction();
		
		menuMain_itemExit.addActionListener(lSymAction);
		menuHelp_itemHelp.addActionListener(lSymAction);
		menuHelp_itemAbout.addActionListener(lSymAction);
		
		jButton.addActionListener(lSymAction);
	}


	// class SymMouse extends java.awt.event.MouseAdapter 
	class SymMouse implements MouseListener {
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

	class SymAction implements ActionListener {
		public void actionPerformed(ActionEvent event) {
			Object object = event.getSource();

			if (object == menuMain_itemExit)
				appExit(event);					
			else if (object == menuHelp_itemAbout)
				about(event);
			else if (object == menuHelp_itemHelp)
				helpClick(event);
			
			else if (object == jButton)
				jButton_actionPerformed(event);			
		}
	}


	class ItemChangeListener implements ItemListener {
		@Override
		public void itemStateChanged(ItemEvent event) {
			if (event.getStateChange() == ItemEvent.SELECTED) {
				Object item = event.getItem();
				// do something with object
			}
		}       
	}

	public void MakeWebRequest() {
		
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

	public void appExit(ActionEvent event) {
		// We don't log exit events ;)
		System.exit(0);
	}

	
	
	public void chatCommand(ActionEvent event, String whichCommand) { 
		jTextAreaSource.append("Menu Chat = command " + whichCommand + ", event: " + event + "\n");
		
		
		
	}
	
	
	public void addEditContact(ActionEvent event, int who) {
		if (who == 0)
			jTextAreaSource.append("Menu Contact => edit \"My Contact\", event: " + event + "\n");
		else if (who > 0)
			jTextAreaSource.append("Menu Contact => add/edit contacts, event: " + event + "\n");
		else if (who < 0)
			jTextAreaSource.append("Menu Contact => import contacts, event: " + event + "\n");
	}
	
	public void viewContact(ActionEvent event) {
	
		jTextAreaSource.append("Menu Contact => view contacts, event: " + event + "\n");
	}


	public void about(ActionEvent event) {
	
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
	
	public void helpClick(ActionEvent event) {
	
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

	void jButton_actionPerformed(ActionEvent event) {
		// to do: code goes here.
		 MakeWebRequest();
		try {
			jTextAreaSource.setText("hallo");
		} catch (Exception e) {
			e.printStackTrace();
		}
	}
	
	Image setJarIncludedImage(String imgstr) {
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
	
}
