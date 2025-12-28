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
import eu.cqrxs.fw.crypt.hash.KeyHash;
import eu.cqrxs.fw.crypt.hash.*;
import eu.cqrxs.fw.zip.ZipType;
import eu.cqrxs.fw.zip.GZ;
import eu.cqrxs.fw.crypt.cipher.CipherEnum;
import eu.cqrxs.fw.crypt.cipher.*;
import eu.cqrxs.fw.crypt.encoding.*;

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
	URL keyUrl, hashUrl, addAlgoUrl, xUrl, fileInUrl, fileOutUrl;
	/// at/net/res/img/crypt/file.png");/
	 		
	public KeyHash keyHash = KeyHash.Hex;
	public ZipType zipType = ZipType.None;
	public CipherEnum cipherEnum = CipherEnum.Aes;
	public String cipherString, encodeString;
	public EncodeEnum encodeType = EncodeEnum.Base64;
	
	Font menuFont, cryptFont;  
	JButton jButton_setPipe, jButton_hashPipe, jButton_encrypt, jButton_decrypt, jButton_randomText, jButton_resetForm;
	JComboBox jComboBox, jComboBox_Hash, jComboBox_Zip, jComboBox_Algo, jComboBox_Encoding;
	JPanel jPanelCenter = new JPanel();
	JTextField jTextField_Key, jTextField_Hash, jTextField_Pipe;
	JTextArea jTextAreaSource, jTextAreaDestination;
	JScrollPane scrollSource, scrollDestination;
	CqrJDialog cqrJDialog;
	ImageViewer imKey, imHash, imAddAlgo, imX, imInFile = new ImageViewer(), imOutFile  = new ImageViewer();

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
		
		menuFont = new Font("Dialog", Font.PLAIN, 12);
		jBar.setFont(menuFont);

		/* Menu Main */		
		menuMain = new JMenu();
		menuMain.setText("Main");
		menuMain.setActionCommand("Main");
		menuMain.setFont(menuFont);
		menuMain.setMnemonic((int)'M');
		jBar.add(menuMain);
		
		menuMain_itemOpen = new JMenuItem();
		menuMain_itemOpen.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuMain_itemOpen.setText("Open...");
		menuMain_itemOpen.setActionCommand("Open...");
		menuMain_itemOpen.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_O, Event.CTRL_MASK));
		menuMain_itemOpen.setMnemonic((int)'O');
		menuMain_itemOpen.setFont(menuFont);
		menuMain.add(menuMain_itemOpen);
		
		menuMain_itemSave = new JMenuItem();
		menuMain_itemSave.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuMain_itemSave.setText("Save");
		menuMain_itemSave.setActionCommand("Save");
		menuMain_itemSave.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_S, Event.CTRL_MASK));
		menuMain_itemSave.setMnemonic((int)'S');
		menuMain_itemSave.setFont(menuFont);
		menuMain.add(menuMain_itemSave);

		menuMain_itemSetPipe = new JMenuItem();
		menuMain_itemSetPipe.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuMain_itemSetPipe.setText("Set Pipe");
		menuMain_itemSetPipe.setActionCommand("SetPipe");
		menuMain_itemSetPipe.setFont(menuFont);
		menuMain.add(menuMain_itemSetPipe);

		menuMain_itemHashKey = new JMenuItem();
		menuMain_itemHashKey.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuMain_itemHashKey.setText("Hash Key");
		menuMain_itemHashKey.setActionCommand("HashKey");
		menuMain_itemHashKey.setFont(menuFont);
		menuMain.add(menuMain_itemHashKey);

		menuMain_itemHashPipe = new JMenuItem();
		menuMain_itemHashPipe.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuMain_itemHashPipe.setText("Hash Pipe");
		menuMain_itemHashPipe.setActionCommand("HashPipe");
		menuMain_itemHashPipe.setFont(menuFont);
		menuMain.add(menuMain_itemHashPipe);

		menuMain_itemEncrypt = new JMenuItem();
		menuMain_itemEncrypt.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuMain_itemEncrypt.setText("Encrypt");
		menuMain_itemEncrypt.setActionCommand("Encrypt");
		menuMain_itemEncrypt.setFont(menuFont);
		menuMain.add(menuMain_itemEncrypt);

		menuMain_itemDecrypt = new JMenuItem();
		menuMain_itemDecrypt.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuMain_itemDecrypt.setText("Decrypt");
		menuMain_itemDecrypt.setActionCommand("Decrypt");
		menuMain_itemDecrypt.setFont(menuFont);
		menuMain.add(menuMain_itemDecrypt);

		menuMain_itemRandomText = new JMenuItem();
		menuMain_itemRandomText.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuMain_itemRandomText.setText("Random Text");
		menuMain_itemRandomText.setActionCommand("RandomText");
		menuMain_itemRandomText.setFont(menuFont);
		menuMain.add(menuMain_itemRandomText);
		
		menuMain_itemReset = new JMenuItem();
		menuMain_itemReset.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuMain_itemReset.setText("Reset");
		menuMain_itemReset.setActionCommand("Reset");
		menuMain_itemReset.setFont(menuFont);
		menuMain.add(menuMain_itemReset);

		menuMain_itemExit.setText("Exit");
		menuMain_itemExit.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_X, Event.ALT_MASK));
		menuMain_itemExit.setActionCommand("Exit");
		menuMain_itemExit.setMnemonic((int)'X');
		menuMain_itemReset.setFont(menuFont);
		menuMain.add(menuMain_itemExit);
		
		/* Menu Compression */
		menuZip =  new JMenu();
		menuZip.setFont(menuFont);
		menuZip.setText("Compress");
		menuZip.setActionCommand("compress");
		jBar.add(menuZip);
				
		menuZip_itemNone = new JMenuItem();
		menuZip_itemNone.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuZip_itemNone.setText("None");
		menuZip_itemNone.setActionCommand("None");
		menuZip_itemNone.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_L, Event.CTRL_MASK));
		menuZip_itemNone.setMnemonic((int)'N');
		menuZip_itemNone.setFont(menuFont);
		menuZip.add(menuZip_itemNone);
		
		menuZip_itemGz = new JMenuItem();
		menuZip_itemGz.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuZip_itemGz.setText("Gzip");
		menuZip_itemGz.setActionCommand("Gzip");
		menuZip_itemGz.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_L, Event.CTRL_MASK));
		menuZip_itemGz.setMnemonic((int)'G');
		menuZip_itemGz.setFont(menuFont);
		menuZip.add(menuZip_itemGz);
		
		menuZip_itemBz = new JMenuItem();
		menuZip_itemBz.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuZip_itemBz.setText("Bzip"); 
		menuZip_itemBz.setActionCommand("Bzip");
		menuZip_itemBz.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_L, Event.CTRL_MASK));
		menuZip_itemBz.setMnemonic((int)'B');
		menuZip_itemBz.setFont(menuFont);
		menuZip.add(menuZip_itemBz);
				
		menuZip_itemZip = new JMenuItem();
		menuZip_itemZip.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuZip_itemZip.setText("Zip");
		menuZip_itemZip.setActionCommand("Zip");
		menuZip_itemZip.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_L, Event.CTRL_MASK));
		menuZip_itemZip.setMnemonic((int)'Z');
		menuZip_itemZip.setFont(menuFont);
		menuZip.add(menuZip_itemZip);		
		
		menuZip_item7z = new JMenuItem();
		menuZip_item7z.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuZip_item7z.setText("7z");
		menuZip_item7z.setActionCommand("7z");
		menuZip_item7z.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_L, Event.CTRL_MASK));
		menuZip_item7z.setEnabled(false);
		menuZip_item7z.setMnemonic((int)'7');
		menuZip_item7z.setFont(menuFont);
		menuZip.add(menuZip_item7z);
		
		menuEncoding = new JMenu();
		menuEncoding.setFont(menuFont);
		menuEncoding.setText("Encoding");
		menuEncoding.setActionCommand("Encoding");
		jBar.add(menuEncoding);
		
		menuEncoding_itemNone = new JMenuItem();
		menuEncoding_itemNone.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuEncoding_itemNone.setText("None");
		menuEncoding_itemNone.setActionCommand("None");
		menuEncoding_itemNone.setFont(menuFont);
		// menuEncoding_itemNone.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_L, Event.CTRL_MASK));
		// menuEncoding_itemNone.setMnemonic((int)'L');
		menuEncoding.add(menuEncoding_itemNone);
		
		menuEncoding_itemBase16 = new JMenuItem();
		menuEncoding_itemBase16.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuEncoding_itemBase16.setText("Base16");
		menuEncoding_itemBase16.setActionCommand("Base16");
		menuEncoding_itemBase16.setFont(menuFont);
		// menuEncoding_itemBase16.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_L, Event.CTRL_MASK));
		// menuEncoding_itemBase16.setMnemonic((int)'L');
		menuEncoding.add(menuEncoding_itemBase16);
		
		menuEncoding_itemHex16 = new JMenuItem();
		menuEncoding_itemHex16.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuEncoding_itemHex16.setText("Hex16");
		menuEncoding_itemHex16.setActionCommand("Hex16");
		menuEncoding_itemHex16.setFont(menuFont);
		// menuEncoding_itemHex16.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_L, Event.CTRL_MASK));
		// menuEncoding_itemHex16.setMnemonic((int)'L');
		menuEncoding.add(menuEncoding_itemHex16);
		
		menuEncoding_itemUu = new JMenuItem();
		menuEncoding_itemUu.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuEncoding_itemUu.setText("Uu");
		menuEncoding_itemUu.setActionCommand("Uu");
		menuEncoding_itemUu.setFont(menuFont);
		// menuEncoding_itemUu.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_L, Event.CTRL_MASK));
		// menuEncoding_itemUu.setMnemonic((int)'L');
		menuEncoding.add(menuEncoding_itemUu);
		
		
		menuEncoding_itemXx = new JMenuItem();
		menuEncoding_itemXx.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuEncoding_itemXx.setText("Uu");
		menuEncoding_itemXx.setActionCommand("Uu");
		menuEncoding_itemXx.setFont(menuFont);
		// menuEncoding_itemXx.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_L, Event.CTRL_MASK));
		// menuEncoding_itemXx.setMnemonic((int)'L');
		menuEncoding.add(menuEncoding_itemXx);
			
			
		menuEncoding_itemBase64 = new JMenuItem();
		menuEncoding_itemBase64.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuEncoding_itemBase64.setText("Base64");
		menuEncoding_itemBase64.setActionCommand("Base64");
		menuEncoding_itemBase64.setFont(menuFont);
		// menuEncoding_itemBase64.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_L, Event.CTRL_MASK));
		// menuEncoding_itemBase64.setMnemonic((int)'L');
		menuEncoding.add(menuEncoding_itemBase64);
		
		
		menuHash = new JMenu();
		menuHash.setFont(menuFont);
		menuHash.setText("Hash");
		menuHash.setActionCommand("Hash");
		jBar.add(menuHash);
		
		menuHash_Ascon256 = new JMenuItem();
		menuHash_Ascon256.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuHash_Ascon256.setText("Ascon256");
		menuHash_Ascon256.setActionCommand("Ascon256");
		menuHash_Ascon256.setFont(menuFont);
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
		menuHash_Xoodyak.setFont(menuFont);
		menuHash.add(menuHash_Whirlpool);
		
		menuOptions = new JMenu();
		menuOptions.setFont(menuFont);
		menuOptions.setText("Options");
		menuOptions.setActionCommand("Options");
		jBar.add(menuOptions);
		
		menuOptions_menuWarnings = new JMenu();
		menuOptions_menuWarnings.setText("Warnings");
		menuOptions_menuWarnings.setActionCommand("Warnings");
		menuOptions_menuWarnings.setFont(menuFont);
		menuOptions.add(menuOptions_menuWarnings);
		
		menuOptions_menuWarnings_itemWarnOnEmptyPipe = new JMenuItem();
		menuOptions_menuWarnings_itemWarnOnEmptyPipe.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuOptions_menuWarnings_itemWarnOnEmptyPipe.setText("Warn on empty pipe");
		menuOptions_menuWarnings_itemWarnOnEmptyPipe.setActionCommand("WarnOnEmptyPipe");
		menuOptions_menuWarnings_itemWarnOnEmptyPipe.setFont(menuFont);
		menuOptions_menuWarnings.add(menuOptions_menuWarnings_itemWarnOnEmptyPipe);
				
		menuOptions_menuWarnings_itemWarnOnDoubleZipping = new JMenuItem();
		menuOptions_menuWarnings_itemWarnOnDoubleZipping.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuOptions_menuWarnings_itemWarnOnDoubleZipping.setText("Warn on double zipping");
		menuOptions_menuWarnings_itemWarnOnDoubleZipping.setActionCommand("WarnOnDoubleZipping");
		menuOptions_menuWarnings_itemWarnOnDoubleZipping.setFont(menuFont);
		menuOptions_menuWarnings.add(menuOptions_menuWarnings_itemWarnOnDoubleZipping);
		
		menuHelp = new JMenu();
		menuHelp.setFont(menuFont);
		menuHelp.setText("?");
		menuHelp.setActionCommand("?");
		menuHelp.setMnemonic((int)'?');		
		jBar.add(menuHelp);
		
		menuHelp_itemAbout = new JMenuItem();
		menuHelp_itemAbout.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuHelp_itemAbout.setText("About...");
		menuHelp_itemAbout.setActionCommand("About");
		menuHelp_itemAbout.setMnemonic((int)'A');
		menuHelp_itemAbout.setFont(menuFont);
		menuHelp.add(menuHelp_itemAbout);
		
		menuHelp_itemHelp = new JMenuItem();
		menuHelp_itemHelp.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuHelp_itemHelp.setText("Help");
		menuHelp_itemHelp.setActionCommand("Help");
		menuHelp_itemHelp.setMnemonic((int)'H');
		menuHelp_itemHelp.setFont(menuFont);
		menuHelp.add(menuHelp_itemHelp);
		
	}
	


	public void Init(JFrame jf) {
		// symantec.itools.lang.Context.setApplet(this);
		
		// getRootPane().putClientProperty("defeatSystemEventQueueCheck", Boolean.TRUE);					
		jf.setLayout(null);
		jf.setSize(1024, 768);
		jf.setResizable(false);
		
		cryptFont = new Font("Dialog", Font.PLAIN, 11);
		SymAction lSymAction = new SymAction();
		SymMouse aSymMouse = new SymMouse();		
		
		jMenuBar = new JMenuBar();
		AddMenus(jMenuBar);
		
		// jMenuBar.setBounds(0, 0, 480, 24);
		
		// jMenuBar.setSize(480,24);
		jf.setJMenuBar(jMenuBar);
		// jf.add(jMenuBar);
		// jMenuBar.move(0,  0);
		
		try {
			keyUrl = new URL("https://area23.at/net/res/img/symbol/key_ring.gif");
			hashUrl = new URL("https://area23.at/net/res/img/crypt/a_hash.png");
			addAlgoUrl = new URL("https://area23.at/net/res/img/crypt/AddAesArrowHover.gif");
			xUrl = new URL("https://area23.at/net/res/img/symbol/close_delete.gif");
			fileInUrl = new URL("https://area23.at/net/res/img/crypt/file.png");
		} catch (MalformedURLException mue) {
			mue.printStackTrace();
		}
		try {
			imKey = new ImageViewer();
			imKey.setImageURL(keyUrl);
			imKey.setBounds(8,28,30,30);	
			imKey.addMouseListener(aSymMouse);			
			jf.getContentPane().add(imKey);
		} catch (Exception ex) {
			ex.printStackTrace();			
		}
		jTextField_Key = new JTextField();
		jTextField_Key.setFont(cryptFont);
		jTextField_Key.setText("zen@area23.at");
		jTextField_Key.setBounds(48,30,640,25);
		jTextField_Key.setFont(cryptFont);		
		jf.getContentPane().add(jTextField_Key);
		
		jButton_setPipe = new JButton();
		jButton_setPipe.setBounds(876,30,120,25);
		jButton_setPipe.setText("Set Pipe");
		jButton_setPipe.setFont(cryptFont);		
		jButton_setPipe.setActionCommand("setPipe");
		jButton_setPipe.addActionListener(lSymAction);
		jf.getContentPane().add(jButton_setPipe);
		
		jComboBox_Hash = new JComboBox(KeyHash.getNames());
		jComboBox_Hash.setBounds(700, 30, 168, 25);
		jComboBox_Hash.setFont(cryptFont);
		jComboBox_Hash.addItemListener(new HashChangeListener());
		jf.getContentPane().add(jComboBox_Hash);
		
		try {
			imHash = new ImageViewer();
			imHash.setImageURL(hashUrl);
			imHash.setBounds(8, 69, 32, 30);		
			imHash.addMouseListener(aSymMouse);
			jf.getContentPane().add(imHash);
		} catch (Exception ex) {
			ex.printStackTrace();			
		}					
		
		jTextField_Hash = new JTextField();
		jTextField_Hash.setFont(cryptFont);
		jTextField_Hash.setText("");		
		jTextField_Hash.setBounds(48,69,823,25);
		// jTextField_Hash.setEnabled(false);
		jTextField_Hash.setEditable(false);
		jTextField_Hash.setFont(cryptFont);
		jTextField_Hash.setBackground(Color.WHITE);  
		jTextField_Hash.setForeground(Color.BLACK);  
		jf.getContentPane().add(jTextField_Hash);
			
		jButton_hashPipe = new JButton();
		jButton_hashPipe.setBounds(876, 69, 120, 25);
		jButton_hashPipe.setActionCommand("hashPipe");
		jButton_hashPipe.setText("Hash Pipe");
		jButton_hashPipe.setFont(cryptFont);		
		jButton_hashPipe.addActionListener(lSymAction);
		jf.getContentPane().add(jButton_hashPipe);	
		
		jComboBox_Zip = new JComboBox(ZipType.getNames());
		jComboBox_Zip.setBounds(8, 112, 96, 25);
		jComboBox_Zip.setFont(cryptFont);
		jComboBox_Zip.addItemListener(new ZipChangeListener());
		jf.getContentPane().add(jComboBox_Zip);

		jComboBox_Algo = new JComboBox(CipherEnum.getNames());
		jComboBox_Algo.setBounds(108, 112, 120, 25);
		jComboBox_Algo.setFont(cryptFont);
		jComboBox_Algo.addItemListener(new CipherChangeListener());
		jf.getContentPane().add(jComboBox_Algo);
				
		try {
			imAddAlgo = new ImageViewer();
			imAddAlgo.setImageURL(addAlgoUrl);
			imAddAlgo.setBounds(230, 111, 32, 27);	
			imAddAlgo.addMouseListener(aSymMouse);
			jf.getContentPane().add(imAddAlgo);
		} catch (Exception ex) {
			ex.printStackTrace();			
		}
			
		jTextField_Pipe = new JTextField();
		jTextField_Pipe.setText("");
		jTextField_Pipe.setBounds(264, 112, 578, 25);
		jTextField_Pipe.setEditable(false);
		// jTextField_Pipe.setEnabled(false);
		jTextField_Pipe.setForeground(Color.BLACK);  
		jTextField_Pipe.setBackground(Color.WHITE);  
		jTextField_Pipe.setFont(cryptFont);
		jf.getContentPane().add(jTextField_Pipe);
		
		
		try {
			imX = new ImageViewer();
			imX.setImageURL(xUrl);
			imX.setBounds(844, 112, 27, 27);	
			imX.addMouseListener(aSymMouse);
			jf.getContentPane().add(imX);
		} catch (Exception ex) {
			ex.printStackTrace();			
		}
				
		jComboBox_Encoding =  new JComboBox(EncodeEnum.getNames());
		jComboBox_Encoding.setBounds(876, 112, 120, 25);
		jComboBox_Encoding.setFont(cryptFont);
		jComboBox_Encoding.addItemListener(new EncodeChangeListener());
		jf.getContentPane().add(jComboBox_Encoding);
		
		
		jButton_encrypt = new JButton();
		jButton_encrypt.setFont(cryptFont);
		jButton_encrypt.setBounds(8, 360, 120, 25);
		jButton_encrypt.setText("Encrypt");
		jButton_encrypt.addActionListener(lSymAction);
		jf.getContentPane().add(jButton_encrypt);
		
		jButton_decrypt = new JButton();
		jButton_decrypt.setFont(cryptFont);
		jButton_decrypt.setBounds(142, 360, 120, 25);
		jButton_decrypt.setText("Decrypt");
		jButton_decrypt.addActionListener(lSymAction);
		jf.getContentPane().add(jButton_decrypt);
		
		jButton_randomText = new JButton();
		jButton_randomText.setFont(cryptFont);
		jButton_randomText.setBounds(368, 360, 120, 25);
		jButton_randomText.setText("Random Text");
		jButton_randomText.addActionListener(lSymAction);
		jf.getContentPane().add(jButton_randomText);
				
		jButton_resetForm = new JButton();
		jButton_resetForm.setFont(cryptFont);
		jButton_resetForm.setBounds(876, 360, 120, 25);
		jButton_resetForm.setText("Reset Form");
		jButton_resetForm.addActionListener(lSymAction);
		jf.getContentPane().add(jButton_resetForm);
		
		
		jTextAreaSource = new JTextArea();
		jTextAreaSource.setBounds(8, 396, 480, 292);
		jTextAreaSource.setBackground(Color.WHITE);  
		jTextAreaSource.setLineWrap(true);
		jTextAreaSource.setFont(cryptFont);
		// jTextAreaSource.append("jMenuBar.getUI() == " + jMenuBar.getUI() + "\n");		
		// scrollSource = new JScrollPane (jTextAreaSource, JScrollPane.VERTICAL_SCROLLBAR_ALWAYS, JScrollPane.HORIZONTAL_SCROLLBAR_ALWAYS);
		jf.getContentPane().add(jTextAreaSource);
				
		jTextAreaDestination = new JTextArea();
		jTextAreaDestination.setBounds(516,396,480,292);
		jTextAreaDestination.setLineWrap(true);
		jTextAreaDestination.setBackground(Color.GRAY);  
		jTextAreaDestination.setEditable(false);
		// jTextAreaDestination.setEnabled(false);
		// scrollDestination = new JScrollPane (jTextAreaDestination, JScrollPane.VERTICAL_SCROLLBAR_ALWAYS, JScrollPane.HORIZONTAL_SCROLLBAR_ALWAYS);	
        // scrollDestination.setHorizontalScrollBarPolicy();			
		jf.getContentPane().add(jTextAreaDestination);
									
		jf.setVisible(true);
		
		menuMain_itemDecrypt.addActionListener(lSymAction);
		menuMain_itemEncrypt.addActionListener(lSymAction);
		menuMain_itemSetPipe.addActionListener(lSymAction);
		menuMain_itemHashKey.addActionListener(lSymAction);
		menuMain_itemHashPipe.addActionListener(lSymAction);
		menuMain_itemReset.addActionListener(lSymAction);
		menuMain_itemRandomText.addActionListener(lSymAction);
		menuMain_itemExit.addActionListener(lSymAction);
		menuHelp_itemHelp.addActionListener(lSymAction);
		menuHelp_itemAbout.addActionListener(lSymAction);
		
	}

	class HashChangeListener implements ItemListener {
		@Override
		public void itemStateChanged(ItemEvent event) {
			if (event.getStateChange() == ItemEvent.SELECTED) {
				Object item = event.getItem();
				String selectedHash = item.toString();
				keyHash = KeyHash.getEnum(selectedHash);
				// do something with object
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
		}       
	}
	
	class ZipChangeListener implements ItemListener {
		@Override
		public void itemStateChanged(ItemEvent event) {
			if (event.getStateChange() == ItemEvent.SELECTED) {
				Object item = event.getItem();
				String selectedZip = item.toString();
				zipType = ZipType.getEnum(selectedZip);
				// do something with object
				String zipTypeString = zipType.toString();
                // TODO: message it
			}
		}       
	}
	
	class CipherChangeListener implements ItemListener {
		@Override
		public void itemStateChanged(ItemEvent event) {
			if (event.getStateChange() == ItemEvent.SELECTED) {
				Object item = event.getItem();
				String selectedCipher = item.toString();
				cipherEnum = CipherEnum.getEnum(selectedCipher);
				// do something with object
				cipherString = cipherEnum.toString();
                // TODO: message it
			}
		}       
	}
	
	class EncodeChangeListener implements ItemListener {
		@Override
		public void itemStateChanged(ItemEvent event) {
			if (event.getStateChange() == ItemEvent.SELECTED) {
				Object item = event.getItem();
				String selectedEncoding = item.toString();
				encodeType = EncodeEnum.getEnum(selectedEncoding);
				// do something with object
				encodeString = encodeType.toString();
                // TODO: message it
			}
		}       
	}

	class SymMouse extends java.awt.event.MouseAdapter {
		public void mouseClicked(java.awt.event.MouseEvent event) {
			Object object = event.getSource();
			if (object == imAddAlgo) {
				cipherString = cipherEnum.toString();
				String pipeText = jTextField_Pipe.getText();
				jTextField_Pipe.setText(pipeText + cipherString + ";");
			} else if (object == imKey) {
				// keyHash.Hash(
			}
			else if (object == imHash) {
				hashKey_action();				
			}
			else if (object == imX) {
				jTextField_Pipe.setText("");
			}
			else {
				
			}
		}
	}
	// class SymMouse extends java.awt.event.MouseAdapter 
	class SymMouse1 implements MouseListener {
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
				exit_action(event);					
			else if (object == menuHelp_itemAbout)
				about_action(event);
			else if (object == menuHelp_itemHelp)
				help_action(event);
			else if (object == menuMain_itemHashKey)
				hashKey_action();
			
			else if (object == jButton_encrypt || object == menuMain_itemEncrypt)
				encrypt_action(event);
			else if (object == jButton_decrypt || object == menuMain_itemDecrypt)
				decrypt_action(event);
			else if (object == jButton_setPipe || object == menuMain_itemSetPipe)
				setPipe_action(event);			
			else if (object == jButton_hashPipe || object == menuMain_itemHashPipe)
				jButton_hashPipe_action(event);
			else if (object == jButton_randomText || object == menuMain_itemRandomText)
				randomText_action(event);
			else if (object == jButton_resetForm || object == menuMain_itemReset)
				resetForm_action(event);
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


	public void exit_action(ActionEvent event) {
		// We don't log exit events ;)
		System.exit(0);
	}
	
	public void about_action(ActionEvent event) {
	
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
	
	public void help_action(ActionEvent event) {
	
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

	void setPipe_action(ActionEvent event) {
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
	
	void hashKey_action() {
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
	
	void jButton_hashPipe_action(ActionEvent event) {
		// to do: code goes here.
		try {
			jTextAreaSource.setText("jButton_hashPipe_action");
		} catch (Exception e) {
			e.printStackTrace();
		}
	}
		
	void randomText_action(ActionEvent event) {
		// to do: code goes here.
		try {
			jTextAreaSource.setText("randomText_action");
		} catch (Exception e) {
			e.printStackTrace();
		}
	}
	
	void resetForm_action(ActionEvent event) {		
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
	
	void encrypt_action(ActionEvent event) {
		
		String plain = jTextAreaSource.getText();
		String key = jTextField_Key.getText();
		String hashed = keyHash.hash(key);
		jTextField_Hash.setText(hashed);
		String cipherPipeString = jTextField_Pipe.getText();
		CipherEnum[] ciphers = new CipherEnum[0];
		if (cipherPipeString.length() > 0) {
			ciphers = CipherEnum.parsePipeText(cipherPipeString);
		}
		CipherPipe pipe = new CipherPipe(ciphers, 8, encodeType, zipType, keyHash);

		String encrypted = "";
		CipherEnum[] cipherEnums = pipe.getInPipe();
		String pipeSting = "";
		for (int ci = 0; ci < cipherEnums.length; ci++)
			pipeSting = pipeSting + cipherEnums[ci].getName() + ";";

		// showMsg(String.format("PipeString: %s \nEncoding: %s Hashing: %s zipping; %s", 
		// 		pipeSting, encodeType.getName(), keyHash.getName(), zipType.getName()), 2, false);
		try {
			// showMsg(String.format("pipe.encrypt with key=%s, hash=%s, \nencode=%s keyHash=%s, zip=%s", 
			// 	key, hashed, encodeType.getName(), keyHash.getName(), zipType.getName()), 4, false);
			encrypted = pipe.encrpytTextGoRounds(plain, key, hashed, encodeType, zipType, keyHash);
			jTextAreaDestination.setText(encrypted);
		} catch (Exception ex) {
			ex.printStackTrace();
			jTextAreaDestination.setText(ex.toString());
		}
	}
	
	void decrypt_action(ActionEvent event) {

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
		// showMsg(String.format("Out pipe: %s \nEncoding: %s Hashing: %s zipping; %s",
		//        pipeSting, encodeType.getName(), keyHash.getName(), zipType.getName()), 1, true);

		String decrypted = "";
		try {
			decrypted = pipe.decryptTextRoundsGo(encrypted, key, hashed, encodeType, zipType, keyHash);
			jTextAreaDestination.setText(decrypted);
			// showMsg(String.format("pipe.decrypt with key=%s, hash=%s, \nencode=%s keyHash=%s, zip=%s",
			//        key, hashed, encodeType.getName(), keyHash.getName(), zipType.getName()), 4, true);
		} catch (Exception ex) {
			jTextAreaDestination.setText(ex.toString());
			ex.printStackTrace();
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
