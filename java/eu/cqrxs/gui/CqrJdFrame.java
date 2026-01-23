/**
 *
 * @author           Heinrich Elsigan
 * @version          V 0.2
 * @since            JDK 8
 *
 * Coded 2021-2025 by
 * <a href="mailto:he@area23.at">Heinrich.Elsigan</a><a href="https://heinrichelsigan.area23.at">heinrichelsigan.area23.at</a>
 */ 
package eu.cqrxs.gui;

import eu.cqrxs.util.Constants;
import eu.cqrxs.crypt.hash.KeyHash;
import eu.cqrxs.crypt.cipher.CipherEnum;
import eu.cqrxs.crypt.cipher.CipherPipe;
import eu.cqrxs.crypt.encoding.EncodeEnum;
import eu.cqrxs.util.Fortune;
import eu.cqrxs.zip.ZipType;

import java.nio.file.Files;
import java.nio.file.Path;
import java.awt.*;
import java.awt.event.*;
import java.io.File;
import java.io.InputStream;
import java.io.BufferedInputStream;
import java.lang.*;
import java.net.http.*;
import java.net.*;
import java.time.Duration;
import javax.swing.*;

/**
 * class CqrJdFrame is main form for PermAgainCrypt in java
 */
public class CqrJdFrame extends JFrame {

	public static CqrJdFrame cqrJdFrame;
	protected static byte[] openFileBytes, saveFileBytes;
	URL keyUrl, hashUrl, addAlgoUrl, xUrl, fileInUrl, fileEnCryptedUrl, fileDeCryptedUrl, pipeUrl;
	/// at/net/res/img/crypt/file.png");/
	 		
	protected KeyHash keyHash = KeyHash.Hex;
	protected eu.cqrxs.zip.ZipType zipType = eu.cqrxs.zip.ZipType.None;
	protected CipherEnum cipherEnum = CipherEnum.Aes;
	protected String cipherString, encodeString, openFileName, saveFileName, saveFileSuffix = "";
	protected EncodeEnum encodeType = EncodeEnum.Base64;
		
	
	JButton jButton_setPipe, jButton_hashPipe, jButton_encrypt, jButton_decrypt, jButton_randomText, jButton_resetForm;
	JComboBox jComboBox, jComboBox_Hash, jComboBox_Zip, jComboBox_Algo, jComboBox_Encoding;
	JPanel jPanelCenter = new JPanel();
	JLabel jLabel_fileIn, jLabel_fileOut, jLabel_infoMessage, jLabel_statusSource, jLabel_statusDestination;
	JTextField jTextField_Key, jTextField_Hash, jTextField_Pipe;
	JTextArea jTextAreaSource, jTextAreaDestination;
	JScrollPane scrollSource, scrollDestination;
	eu.cqrxs.gui.CqrJDialog cqrJDialog;
	eu.cqrxs.gui.ImageViewer imKey, imHash, imAddAlgo, imX, imInFile = new eu.cqrxs.gui.ImageViewer(), imOutFile = new eu.cqrxs.gui.ImageViewer();
	
	Font menuFont, cryptFont, monoSpaceFont, monoSpaced = new Font("Monospaced", Font.PLAIN, 10);
	static Color defaultMenuItemBg, selectionBg;
	
	JMenuBar jBar = new JMenuBar();
	// JMenuBar jMenuBar = new JMenuBar();
	JMenu menuMain, menuZip, menuEncoding, menuHash, menuOptions, menuHelp = new JMenu();
	
	JMenuItem menuMain_itemOpen, menuMain_itemSave, 
				menuMain_itemSetPipe, menuMain_itemHashKey, menuMain_itemHashPipe, 
				menuMain_itemEncrypt, menuMain_itemDecrypt, menuMain_itemRandomText, menuMain_itemReset,
				menuMain_itemExit = new JMenuItem();

	JMenuItem menuZip_item7z, menuZip_itemGz, menuZip_itemBz, menuZip_itemZip, menuZip_itemNone;
	
	JMenuItem menuEncoding_itemNone, menuEncoding_itemBase16, menuEncoding_itemHex16, menuEncoding_itemUu, menuEncoding_itemXx, menuEncoding_itemBase64;
	
	JMenuItem menuHash_Ascon256, menuHash_Blake2xs, menuHash_BCrypt, menuHash_CShake, menuHash_MD5, menuHash_Hex, menuHash_OpenBSDCrypt,
				menuHash_RipeMD256, menuHash_Sha1, menuHash_Sha256, menuHash_Sha512, menuHash_SCrypt, menuHash_Whirlpool, menuHash_Xoodyak;

	JMenu menuOptions_menuWarnings, menuOptions_verifyEncryption, menuOptions_menuFileSettings;
	JMenuItem menuOptions_menuWarnings_itemWarnOnEmptyPipe, menuOptions_menuWarnings_itemWarnOnDoubleZipping;
	
	JMenuItem menuHelp_itemAbout = new JMenuItem(), menuHelp_itemHelp = new JMenuItem();		
	//}}

    /**
     * main entry method
     * @param args command line arguments
     */
	public static void main(String args[]) {
		cqrJdFrame = new CqrJdFrame();
	}
		
    /**
     * main constructor for CqrJdFrame
     */
	public CqrJdFrame() {
		setLayout(null);
		setSize(1024,768);
		Init();
		setVisible(true);
		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
	}


    /**
     * AddMenus add all menus
     * @param jbar main menu bar
     * @returns {@link JMenuBar}
     */
	public JMenuBar AddMenus(SymAction aSymAction) {
	    
        jBar = new JMenuBar();	
		menuFont = new Font("Dialog", Font.PLAIN, 12);
		jBar.setFont(menuFont);

		defaultMenuItemBg = UIManager.getColor("MenuItem.background");
		// Get the color when a menu item is selected/hovered
		selectionBg = UIManager.getColor("MenuItem.selectionBackground");

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
		menuMain_itemOpen.addActionListener(aSymAction);
		menuMain.add(menuMain_itemOpen);
		
		menuMain_itemSave = new JMenuItem();
		menuMain_itemSave.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuMain_itemSave.setText("Save");
		menuMain_itemSave.setActionCommand("Save");
		menuMain_itemSave.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_S, Event.CTRL_MASK));
		menuMain_itemSave.setMnemonic((int)'S');
		menuMain_itemSave.setFont(menuFont);
		menuMain_itemSave.addActionListener(aSymAction);
		menuMain.add(menuMain_itemSave);

		menuMain_itemSetPipe = new JMenuItem();
		menuMain_itemSetPipe.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuMain_itemSetPipe.setText("Set Pipe");
		menuMain_itemSetPipe.setActionCommand("SetPipe");
		menuMain_itemSetPipe.setFont(menuFont);
		menuMain_itemSetPipe.addActionListener(aSymAction);
		menuMain.add(menuMain_itemSetPipe);

		menuMain_itemHashKey = new JMenuItem();
		menuMain_itemHashKey.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuMain_itemHashKey.setText("Hash Key");
		menuMain_itemHashKey.setActionCommand("HashKey");
		menuMain_itemHashKey.setFont(menuFont);
		menuMain_itemHashKey.addActionListener(aSymAction);
		menuMain.add(menuMain_itemHashKey);

		menuMain_itemHashPipe = new JMenuItem();
		menuMain_itemHashPipe.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuMain_itemHashPipe.setText("Hash Pipe");
		menuMain_itemHashPipe.setActionCommand("HashPipe");
		menuMain_itemHashPipe.setFont(menuFont);
		menuMain_itemHashPipe.addActionListener(aSymAction);
		menuMain.add(menuMain_itemHashPipe);

		menuMain_itemEncrypt = new JMenuItem();
		menuMain_itemEncrypt.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuMain_itemEncrypt.setText("Encrypt");
		menuMain_itemEncrypt.setActionCommand("Encrypt");
		menuMain_itemEncrypt.setFont(menuFont);
		menuMain_itemEncrypt.addActionListener(aSymAction);
		menuMain.add(menuMain_itemEncrypt);

		menuMain_itemDecrypt = new JMenuItem();
		menuMain_itemDecrypt.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuMain_itemDecrypt.setText("Decrypt");
		menuMain_itemDecrypt.setActionCommand("Decrypt");
		menuMain_itemDecrypt.setFont(menuFont);
		menuMain_itemDecrypt.addActionListener(aSymAction);
		menuMain.add(menuMain_itemDecrypt);

		menuMain_itemRandomText = new JMenuItem();
		menuMain_itemRandomText.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuMain_itemRandomText.setText("Random Text");
		menuMain_itemRandomText.setActionCommand("RandomText");
		menuMain_itemRandomText.setFont(menuFont);
		menuMain_itemRandomText.addActionListener(aSymAction);
		menuMain.add(menuMain_itemRandomText);
		
		menuMain_itemReset = new JMenuItem();
		menuMain_itemReset.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuMain_itemReset.setText("Reset");
		menuMain_itemReset.setActionCommand("Reset");
		menuMain_itemReset.setFont(menuFont);
		menuMain_itemReset.addActionListener(aSymAction);
		menuMain.add(menuMain_itemReset);

		menuMain_itemExit.setText("Exit");
		menuMain_itemExit.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_X, Event.ALT_MASK));
		menuMain_itemExit.setActionCommand("Exit");
		menuMain_itemExit.setMnemonic((int)'X');
		menuMain_itemExit.setFont(menuFont);
		menuMain_itemExit.addActionListener(aSymAction);
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
		menuZip_itemNone.addActionListener(aSymAction);
		menuZip.add(menuZip_itemNone);
		
		menuZip_itemGz = new JMenuItem();
		menuZip_itemGz.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuZip_itemGz.setText("GZip");
		menuZip_itemGz.setActionCommand("GZip");
		menuZip_itemGz.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_L, Event.CTRL_MASK));
		menuZip_itemGz.setMnemonic((int)'G');
		menuZip_itemGz.setFont(menuFont);
		menuZip_itemGz.addActionListener(aSymAction);
		menuZip.add(menuZip_itemGz);
		
		menuZip_itemBz = new JMenuItem();
		menuZip_itemBz.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuZip_itemBz.setText("BZip2"); 
		menuZip_itemBz.setActionCommand("BZip2");
		menuZip_itemBz.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_L, Event.CTRL_MASK));
		menuZip_itemBz.setMnemonic((int)'B');
		menuZip_itemBz.setFont(menuFont);
		menuZip_itemBz.addActionListener(aSymAction);
		menuZip.add(menuZip_itemBz);
				
		menuZip_itemZip = new JMenuItem();
		menuZip_itemZip.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuZip_itemZip.setText("Zip");
		menuZip_itemZip.setActionCommand("Zip");
		menuZip_itemZip.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_L, Event.CTRL_MASK));
		menuZip_itemZip.setMnemonic((int)'Z');
		menuZip_itemZip.setFont(menuFont);
		menuZip_itemZip.addActionListener(aSymAction);
		menuZip.add(menuZip_itemZip);		
		
		menuZip_item7z = new JMenuItem();
		menuZip_item7z.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuZip_item7z.setText("7z");
		menuZip_item7z.setActionCommand("7z");
		menuZip_item7z.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_L, Event.CTRL_MASK));
		menuZip_item7z.setEnabled(false);
		menuZip_item7z.setMnemonic((int)'7');
		menuZip_item7z.setFont(menuFont);
		menuZip_item7z.addActionListener(aSymAction);
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
		menuEncoding_itemNone.addActionListener(aSymAction);
		menuEncoding.add(menuEncoding_itemNone);
		
		menuEncoding_itemBase16 = new JMenuItem();
		menuEncoding_itemBase16.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuEncoding_itemBase16.setText("Base16");
		menuEncoding_itemBase16.setActionCommand("Base16");
		menuEncoding_itemBase16.setFont(menuFont);
		menuEncoding_itemBase16.addActionListener(aSymAction);
		menuEncoding.add(menuEncoding_itemBase16);
		
		menuEncoding_itemHex16 = new JMenuItem();
		menuEncoding_itemHex16.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuEncoding_itemHex16.setText("Hex16");
		menuEncoding_itemHex16.setActionCommand("Hex16");
		menuEncoding_itemHex16.setFont(menuFont);
		menuEncoding_itemHex16.addActionListener(aSymAction);
		menuEncoding.add(menuEncoding_itemHex16);
		
		menuEncoding_itemUu = new JMenuItem();
		menuEncoding_itemUu.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuEncoding_itemUu.setText("Uu");
		menuEncoding_itemUu.setActionCommand("Uu");
		menuEncoding_itemUu.setFont(menuFont);
		menuEncoding_itemUu.addActionListener(aSymAction);
		menuEncoding.add(menuEncoding_itemUu);
		
		
		menuEncoding_itemXx = new JMenuItem();
		menuEncoding_itemXx.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuEncoding_itemXx.setText("Xx");
		menuEncoding_itemXx.setActionCommand("Xx");
		menuEncoding_itemXx.setFont(menuFont);		
		menuEncoding_itemXx.addActionListener(aSymAction);
		menuEncoding.add(menuEncoding_itemXx);
			
			
		menuEncoding_itemBase64 = new JMenuItem();
		menuEncoding_itemBase64.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuEncoding_itemBase64.setText("Base64");
		menuEncoding_itemBase64.setActionCommand("Base64");
		menuEncoding_itemBase64.setFont(menuFont);		
		menuEncoding_itemBase64.addActionListener(aSymAction);
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
		
		menuHash_BCrypt = new JMenuItem();
		menuHash_BCrypt.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuHash_BCrypt.setText("BCrypt");
		menuHash_BCrypt.setActionCommand("BCrypt");
		menuHash_BCrypt.setFont(menuFont);
		menuHash_BCrypt.addActionListener(aSymAction);		
		menuHash.add(menuHash_BCrypt);
				
		menuHash_Blake2xs = new JMenuItem();
		menuHash_Blake2xs.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuHash_Blake2xs.setText("Blake2xs");
		menuHash_Blake2xs.setActionCommand("Blake2xs");
		menuHash_Blake2xs.setFont(menuFont);
		menuHash_Blake2xs.addActionListener(aSymAction);
		menuHash.add(menuHash_Blake2xs);

		menuHash_CShake = new JMenuItem();
		menuHash_CShake.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuHash_CShake.setText("CShake");
		menuHash_CShake.setActionCommand("CShake");
		menuHash_CShake.setFont(menuFont);
		menuHash_CShake.addActionListener(aSymAction);
		menuHash.add(menuHash_CShake);
		
		menuHash_Hex = new JMenuItem();
		menuHash_Hex.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuHash_Hex.setText("Hex");
		menuHash_Hex.setActionCommand("Hex");
		menuHash_Hex.setFont(menuFont);
		menuHash_Hex.addActionListener(aSymAction);
		menuHash.add(menuHash_Hex);
		
		menuHash_MD5 = new JMenuItem();
		menuHash_MD5.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuHash_MD5.setText("MD5");
		menuHash_MD5.setActionCommand("MD5");
		menuHash_MD5.setFont(menuFont);
		menuHash_MD5.addActionListener(aSymAction);
		menuHash.add(menuHash_MD5);				
		
		menuHash_OpenBSDCrypt = new JMenuItem();
		menuHash_OpenBSDCrypt.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuHash_OpenBSDCrypt.setText("OpenBSDCrypt");
		menuHash_OpenBSDCrypt.setActionCommand("OpenBSDCrypt");
		menuHash_OpenBSDCrypt.setFont(menuFont);
		menuHash_OpenBSDCrypt.addActionListener(aSymAction);
		menuHash.add(menuHash_OpenBSDCrypt);
		
		menuHash_RipeMD256 = new JMenuItem();
		menuHash_RipeMD256.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuHash_RipeMD256.setText("RipeMD256");
		menuHash_RipeMD256.setActionCommand("RipeMD256");
		menuHash_RipeMD256.setFont(menuFont);
		menuHash_RipeMD256.addActionListener(aSymAction);
		menuHash.add(menuHash_RipeMD256);
						
		menuHash_SCrypt = new JMenuItem();
		menuHash_SCrypt.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuHash_SCrypt.setText("SCrypt");
		menuHash_SCrypt.setActionCommand("SCrypt");
		menuHash_SCrypt.setFont(menuFont);
		menuHash_SCrypt.addActionListener(aSymAction);
		menuHash.add(menuHash_SCrypt);
		
		menuHash_Sha1 = new JMenuItem();
		menuHash_Sha1.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuHash_Sha1.setText("Sha1");
		menuHash_Sha1.setActionCommand("Sha1");
		menuHash_Sha1.setFont(menuFont);
		menuHash_Sha1.addActionListener(aSymAction);
		menuHash.add(menuHash_Sha1);		
		
		menuHash_Sha256 = new JMenuItem();
		menuHash_Sha256.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuHash_Sha256.setText("Sha256");
		menuHash_Sha256.setActionCommand("Sha256");
		menuHash_Sha256.setFont(menuFont);
		menuHash_Sha256.addActionListener(aSymAction);
		menuHash.add(menuHash_Sha256);
		
		menuHash_Sha512 = new JMenuItem();
		menuHash_Sha512.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuHash_Sha512.setText("Sha512");
		menuHash_Sha512.setActionCommand("Sha512");
		menuHash_Sha512.setFont(menuFont);
		menuHash_Sha512.addActionListener(aSymAction);
		menuHash.add(menuHash_Sha512);
				
		menuHash_Whirlpool = new JMenuItem();
		menuHash_Whirlpool.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuHash_Whirlpool.setText("Whirlpool");
		menuHash_Whirlpool.setActionCommand("Whirlpool");
		menuHash_Whirlpool.setFont(menuFont);
		menuHash_Whirlpool.addActionListener(aSymAction);
		menuHash.add(menuHash_Whirlpool);
		
		menuHash_Xoodyak = new JMenuItem();
		menuHash_Xoodyak.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuHash_Xoodyak.setText("Xoodyak");
		menuHash_Xoodyak.setActionCommand("Xoodyak");
		menuHash_Xoodyak.setFont(menuFont);
		menuHash_Xoodyak.addActionListener(aSymAction);
		menuHash.add(menuHash_Xoodyak);
		
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
		menuHelp_itemAbout.addActionListener(aSymAction);
		menuHelp.add(menuHelp_itemAbout);
		
		menuHelp_itemHelp = new JMenuItem();
		menuHelp_itemHelp.setHorizontalTextPosition(SwingConstants.RIGHT);
		menuHelp_itemHelp.setText("Help");
		menuHelp_itemHelp.setActionCommand("Help");
		menuHelp_itemHelp.setMnemonic((int)'H');
		menuHelp_itemHelp.setFont(menuFont);
		menuHelp_itemHelp.addActionListener(aSymAction);
		menuHelp.add(menuHelp_itemHelp);
		
        return jBar;
	}
	


	public void Init() {
		
		// getRootPane().putClientProperty("defeatSystemEventQueueCheck", Boolean.TRUE);					
		setLayout(null);
		setSize(1024, 768);
		setResizable(false);
		
        monoSpaceFont = new Font(Font.MONOSPACED, Font.PLAIN, 11);
		cryptFont = new Font("Dialog", Font.PLAIN, 11);
		SymAction lSymAction = new SymAction();
		SymMouse aSymMouse = new SymMouse();		
		
		jBar = AddMenus(lSymAction);
		setJMenuBar(jBar);
		
		try {
			keyUrl = new URL("https://area23.at/net/res/img/symbol/key_ring.gif");
			hashUrl = new URL("https://area23.at/net/res/img/crypt/a_hash.png");
			addAlgoUrl = new URL("https://area23.at/net/res/img/crypt/AddAesArrowHover.gif");
			xUrl = new URL("https://area23.at/net/res/img/symbol/close_delete.gif");
			fileInUrl = new URL("https://area23.at/net/res/img/crypt/file.png");
			fileEnCryptedUrl = new URL("https://area23.at/net/res/img/crypt/encrypted.png");
			fileDeCryptedUrl = new URL("https://area23.at/net/res/img/crypt/decrypted.png");
		} catch (MalformedURLException mue) {
			mue.printStackTrace();
		}
		try {
			imKey = new eu.cqrxs.gui.ImageViewer();
			imKey.setImageURL(keyUrl);
			imKey.setBounds(8,28,30,30);	
			imKey.addMouseListener(aSymMouse);			
			getContentPane().add(imKey);
		} catch (Exception ex) {
			ex.printStackTrace();			
		}
		jTextField_Key = new JTextField();
		jTextField_Key.setFont(cryptFont);
		jTextField_Key.setText("zen@area23.at");
		jTextField_Key.setBounds(48,30,640,25);
		jTextField_Key.setFont(cryptFont);		
		getContentPane().add(jTextField_Key);
		
		jButton_setPipe = new JButton();
		jButton_setPipe.setBounds(876,30,120,25);
		jButton_setPipe.setText("Set Pipe");
		jButton_setPipe.setFont(cryptFont);		
		jButton_setPipe.setActionCommand("setPipe");
		jButton_setPipe.addActionListener(lSymAction);
		getContentPane().add(jButton_setPipe);
		
		jComboBox_Hash = new JComboBox(KeyHash.getNames());
		jComboBox_Hash.setBounds(700, 30, 168, 25);
		jComboBox_Hash.setFont(cryptFont);
		jComboBox_Hash.addItemListener(new HashChangeListener());
		getContentPane().add(jComboBox_Hash);
		
		try {
			imHash = new eu.cqrxs.gui.ImageViewer();
			imHash.setImageURL(hashUrl);
			imHash.setBounds(8, 69, 32, 30);		
			imHash.addMouseListener(aSymMouse);
			getContentPane().add(imHash);
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
		getContentPane().add(jTextField_Hash);
			
		jButton_hashPipe = new JButton();
		jButton_hashPipe.setBounds(876, 69, 120, 25);
		jButton_hashPipe.setActionCommand("hashPipe");
		jButton_hashPipe.setText("Hash Pipe");
		jButton_hashPipe.setFont(cryptFont);		
		jButton_hashPipe.addActionListener(lSymAction);
		getContentPane().add(jButton_hashPipe);	
		
		jComboBox_Zip = new JComboBox(ZipType.getNames());
		jComboBox_Zip.setBounds(8, 112, 96, 25);
		jComboBox_Zip.setFont(cryptFont);
		jComboBox_Zip.addItemListener(new ZipChangeListener());
		getContentPane().add(jComboBox_Zip);

		jComboBox_Algo = new JComboBox(CipherEnum.getNames());
		jComboBox_Algo.setBounds(108, 112, 120, 25);
		jComboBox_Algo.setFont(cryptFont);
		jComboBox_Algo.addItemListener(new CipherChangeListener());
		getContentPane().add(jComboBox_Algo);
				
		try {
			imAddAlgo = new eu.cqrxs.gui.ImageViewer();
			imAddAlgo.setImageURL(addAlgoUrl);
			imAddAlgo.setBounds(230, 111, 32, 27);	
			imAddAlgo.addMouseListener(aSymMouse);
			getContentPane().add(imAddAlgo);
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
		getContentPane().add(jTextField_Pipe);
		
		
		try {
			imX = new eu.cqrxs.gui.ImageViewer();
			imX.setImageURL(xUrl);
			imX.setBounds(844, 112, 27, 27);	
			imX.addMouseListener(aSymMouse);
			getContentPane().add(imX);
		} catch (Exception ex) {
			ex.printStackTrace();			
		}
				
		jComboBox_Encoding =  new JComboBox(EncodeEnum.getNames());
		jComboBox_Encoding.setBounds(876, 112, 120, 25);
		jComboBox_Encoding.setFont(cryptFont);
		jComboBox_Encoding.addItemListener(new EncodeChangeListener());
		getContentPane().add(jComboBox_Encoding);
		selectItemByString(jComboBox_Encoding, menuEncoding, "Base64");
		
		try {
			imInFile = new ImageViewer();
			imInFile.setImageURL(fileInUrl);
			imInFile.setBounds(8, 144 ,60,60);	
			imInFile.addMouseListener(aSymMouse);			
			getContentPane().add(imInFile);
		} catch (Exception ex) {
			ex.printStackTrace();			
		}
		jLabel_fileIn = new JLabel();
		jLabel_fileIn.setFont(cryptFont);
		jLabel_fileIn.setBounds(8, 208, 120, 24);
		jLabel_fileIn.setText("[No input file loaded]");
		jLabel_fileIn.setFont(cryptFont);		
		getContentPane().add(jLabel_fileIn);

		try {
			imOutFile = new ImageViewer();
			imOutFile.setImageURL(fileInUrl);
			imOutFile.setBounds(912, 144, 60, 60);		
			imOutFile.addMouseListener(aSymMouse);
			getContentPane().add(imOutFile);
		} catch (Exception ex) {
			ex.printStackTrace();			
		}					
		
		jLabel_fileOut = new JLabel();
		jLabel_fileOut.setFont(cryptFont);
		jLabel_fileOut.setBounds(884, 208, 120, 24);
		jLabel_fileOut.setText("[No output file processed]");		
		getContentPane().add(jLabel_fileOut);				
				
		
		jButton_encrypt = new JButton();
		jButton_encrypt.setFont(cryptFont);
		jButton_encrypt.setBounds(8, 244, 120, 25);
		jButton_encrypt.setText("Encrypt");
		jButton_encrypt.addActionListener(lSymAction);
		getContentPane().add(jButton_encrypt);
		
		jButton_decrypt = new JButton();
		jButton_decrypt.setFont(cryptFont);
		jButton_decrypt.setBounds(142, 244, 120, 25);
		jButton_decrypt.setText("Decrypt");
		jButton_decrypt.addActionListener(lSymAction);
		getContentPane().add(jButton_decrypt);
		
		jButton_randomText = new JButton();
		jButton_randomText.setFont(cryptFont);
		jButton_randomText.setBounds(368, 244, 120, 25);
		jButton_randomText.setText("Random Text");
		jButton_randomText.addActionListener(lSymAction);
		getContentPane().add(jButton_randomText);
				
		jLabel_infoMessage = new JLabel();
		jLabel_infoMessage.setFont(cryptFont);
		jLabel_infoMessage.setBounds(512, 244, 468, 25);
		jLabel_infoMessage.setText("");
		jLabel_infoMessage.setBackground(Color.YELLOW);
		getContentPane().add(jLabel_infoMessage);
				
		jButton_resetForm = new JButton();
		jButton_resetForm.setFont(cryptFont);
		jButton_resetForm.setBounds(876, 244, 120, 25);
		jButton_resetForm.setText("Reset Form");
		jButton_resetForm.addActionListener(lSymAction);
		getContentPane().add(jButton_resetForm);
		
		
		jTextAreaSource = new JTextArea();
		jTextAreaSource.setBounds(8, 280, 480, 400);
		jTextAreaSource.setBackground(Color.WHITE);  
		jTextAreaSource.setFont(cryptFont);
		jTextAreaSource.setLineWrap(true);
		jTextAreaSource.setFont(monoSpaceFont);
		// jTextAreaSource.append("jMenuBar.getUI() == " + jMenuBar.getUI() + "\n");		
		// scrollSource = new JScrollPane (jTextAreaSource, JScrollPane.VERTICAL_SCROLLBAR_ALWAYS, JScrollPane.HORIZONTAL_SCROLLBAR_ALWAYS);
		getContentPane().add(jTextAreaSource);
				
		jTextAreaDestination = new JTextArea();
		jTextAreaDestination.setBounds(516, 280, 480, 400);
		jTextAreaDestination.setLineWrap(true);
		jTextAreaDestination.setFont(monoSpaced);
		jTextAreaDestination.setEditable(false);
		jTextAreaDestination.setEditable(false);
		// jTextAreaDestination.setEnabled(false);
		// scrollDestination = new JScrollPane (jTextAreaDestination, JScrollPane.VERTICAL_SCROLLBAR_ALWAYS, JScrollPane.HORIZONTAL_SCROLLBAR_ALWAYS);	
        // scrollDestination.setHorizontalScrollBarPolicy();			
		getContentPane().add(jTextAreaDestination);
		
		jLabel_statusSource = new JLabel();
		jLabel_statusSource.setBounds(8, 684, 120, 25);
		jLabel_statusSource.setFont(cryptFont);
		jLabel_statusSource.setText("");
		getContentPane().add(jLabel_statusSource);
		
		jLabel_statusDestination = new JLabel();
		jLabel_statusDestination.setBounds(876, 684, 120, 25);
		jLabel_statusDestination.setFont(cryptFont);
		jLabel_statusDestination.setText("");
		getContentPane().add(jLabel_statusDestination);
		
		setVisible(true);	
		
	}

	protected class HashChangeListener implements ItemListener {
		@Override
		public void itemStateChanged(ItemEvent event) {
			if (event.getStateChange() == ItemEvent.SELECTED) {
				Object item = event.getItem();
				String selectedHash = item.toString();
				selectMenuItemByString(menuHash, selectedHash);
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
	
	protected class ZipChangeListener implements ItemListener {
		@Override
		public void itemStateChanged(ItemEvent event) {
			if (event.getStateChange() == ItemEvent.SELECTED) {
				Object item = event.getItem();
				String selectedZip = item.toString();
				selectMenuItemByString(menuZip, selectedZip);
				zipType = ZipType.getEnum(selectedZip);
				// do something with object
				String zipTypeString = zipType.toString();
                // TODO: message it
			}
		}       
	}
	
	protected class CipherChangeListener implements ItemListener {
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
	
	protected class EncodeChangeListener implements ItemListener {
		@Override
		public void itemStateChanged(ItemEvent event) {
			if (event.getStateChange() == ItemEvent.SELECTED) {
				Object item = event.getItem();
				String selectedEncoding = item.toString();
				selectMenuItemByString(menuEncoding, selectedEncoding);
				encodeType = EncodeEnum.getEnum(selectedEncoding);
				// do something with object
				encodeString = encodeType.toString();
                // TODO: message it
			}
		}       
	}


	protected class SymMouse extends java.awt.event.MouseAdapter {
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
			else if (object == imInFile) {
				if (openFileBytes == null || openFileBytes.length < 1) 
					open_action();
			}
			else if (object == imOutFile) {
				if (saveFileBytes == null || saveFileBytes.length < 1) 
					save_action();
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
			else if (object == menuMain_itemHashKey)
				hashKey_action();
			else if (object == menuMain_itemOpen)
				open_action();
			else if (object == menuMain_itemSave)
				save_action();
			else if (object == menuZip_itemNone) 
				selectItemByString(jComboBox_Zip, menuZip, "None");
			else if (object == menuZip_itemGz)
				selectItemByString(jComboBox_Zip, menuZip, "GZip");
			else if (object == menuZip_itemZip) 
				selectItemByString(jComboBox_Zip, menuZip, "Zip"); 
			else if (object == menuZip_itemBz)
				selectItemByString(jComboBox_Zip, menuZip, "BZip2");
			else if (object == menuZip_item7z)
				selectItemByString(jComboBox_Zip, menuZip, "7z"); 
			else if (object == menuEncoding_itemNone)
				selectItemByString(jComboBox_Encoding, menuEncoding, "None"); 
			else if (object == menuEncoding_itemBase16)
				selectItemByString(jComboBox_Encoding, menuEncoding, "Base16"); 
			else if (object == menuEncoding_itemHex16)
				selectItemByString(jComboBox_Encoding, menuEncoding, "Hex16"); 
			else if (object == menuEncoding_itemUu)
				selectItemByString(jComboBox_Encoding, menuEncoding, "Uu"); 
			else if (object == menuEncoding_itemXx)
				selectItemByString(jComboBox_Encoding, menuEncoding, "Xx"); 
			else if (object == menuEncoding_itemBase64)
				selectItemByString(jComboBox_Encoding, menuEncoding, "Base64"); 
			
			else if (object == menuHash_Ascon256) 
				selectItemByString(jComboBox_Hash, menuHash, "Ascon256");								
			else if (object == menuHash_BCrypt) 
				selectItemByString(jComboBox_Hash, menuHash, "BCrypt"); 
			else if (object == menuHash_Blake2xs) 
				selectItemByString(jComboBox_Hash, menuHash, "Blake2xs");					
			else if (object == menuHash_CShake) 
				selectItemByString(jComboBox_Hash, menuHash, "CShake");	
			else if (object == menuHash_Hex) 
				selectItemByString(jComboBox_Hash, menuHash, "Hex"); 	
			else if (object == menuHash_MD5) 
				selectItemByString(jComboBox_Hash, menuHash, "MD5"); 
			else if (object == menuHash_OpenBSDCrypt) 	
				selectItemByString(jComboBox_Hash, menuHash, "OpenBSDCrypt"); 
			else if (object == menuHash_RipeMD256) 
				selectItemByString(jComboBox_Hash, menuHash, "RipeMD256");	
			else if (object == menuHash_SCrypt) 
				selectItemByString(jComboBox_Hash, menuHash, "SCrypt"); 				
			else if (object == menuHash_Sha1) 
				selectItemByString(jComboBox_Hash, menuHash, "Sha1"); 
			else if (object == menuHash_Sha256) 	
				selectItemByString(jComboBox_Hash, menuHash, "Sha256"); 
			else if (object == menuHash_Sha512) 
				selectItemByString(jComboBox_Hash, menuHash, "Sha512");	
			else if (object == menuHash_Whirlpool) 	
				selectItemByString(jComboBox_Hash, menuHash, "Whirlpool"); 
			else if (object == menuHash_Xoodyak) 	
				selectItemByString(jComboBox_Hash, menuHash, "Xoodyak"); 
			
			else if (object == menuHelp_itemAbout)
				about_action(event);
			else if (object == menuHelp_itemHelp)
				help_action(event);

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

	protected void open_action() {                                 
		
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
        openFileName = filename;
		for (int r = (filename.length() -1); r > -1 ; r--) {
			if (filename.charAt(r) == '\\'  || filename.charAt(r) == '/') {
				openFileName = filename.substring(r + 1, filename.length());
				dbgMsg("openFileName = " + openFileName + " index r = " + r, 1, true);
                break;
			}
		}		
		
		try{
			openFileBytes = Files.readAllBytes(f.toPath());			
			saveFileBytes = new byte[0];
			jLabel_fileIn.setText(openFileName);
			jButton_encrypt.requestFocus();
		} catch (Exception e){
			setInfoMsg("Exception during file open.");
			JOptionPane.showMessageDialog(null, e);
			e.printStackTrace();
		}                
		
		if (openFileBytes.length < 2048)
			jLabel_statusSource.setText(openFileBytes.length + " bytes");
		if (openFileBytes.length > 2048 && openFileBytes.length < 1048576)
			jLabel_statusSource.setText((int)(openFileBytes.length / 1024) + " KB.");
		if (openFileBytes.length > 1048576)
			jLabel_statusSource.setText((int)(openFileBytes.length / (1024*1024)) + " MB.");
				
    }   


	protected void save_action() {     
		
		String initDirectory = (java.io.File.separatorChar == '/') ? System.getenv("HOME") : System.getenv("USERPROFILE");
		JFileChooser chooser = new JFileChooser();
		chooser.setCurrentDirectory(new File(initDirectory));
        // chooser.setFileFilter(new FileNameExtensionFilter("save file", saveFileSuffix));
		int fileDialogResult = chooser.showSaveDialog(cqrJdFrame);
		if (fileDialogResult == JFileChooser.CANCEL_OPTION || fileDialogResult == JFileChooser.ERROR_OPTION) {
			dbgMsg("save_action JFileChooser returned: " + fileDialogResult, 2, true);
			return;
        }
	
		File f = chooser.getSelectedFile();	
		
		Path filePath = f.toPath();
		String filename = f.getAbsolutePath();
		for (int r = (filename.length() -1); r <=0 ; r--) {
			if (filename.charAt(r) == '\\'  || filename.charAt(r) == '/') {
				saveFileName = filename.substring(r);
				break;
			}
        }
		try {
			if (saveFileBytes != null && saveFileBytes.length > 0) {
				Files.write(filePath, saveFileBytes);
			    jLabel_fileIn.setText(saveFileName);
			}
			else 
				throw new java.lang.IllegalStateException("saveFileBytes is null or len == 0");
		} catch (Exception ex) {
			setInfoMsg("Exception during file save.");
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
			
			
			setInfoMsg("Set pipe to: " + pipe.getPipeString());
			
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
			setInfoMsg("Hashed key " + keyValue);
		} catch (Exception exh) {
		}
	}
	
	protected void hashPipe_action(ActionEvent event) {
		try {
			String key = jTextField_Key.getText().toString();
			String hashed = keyHash.hash(key);
			jTextField_Hash.setText(hashed);

			CipherPipe pipe = new CipherPipe(hashed, key, encodeType, zipType, keyHash);

			CipherEnum[] cipherEnums = pipe.getInPipe();
			String pipeSting = "";
			for (int ci = 0; ci < cipherEnums.length; ci++)
				pipeSting = pipeSting + cipherEnums[ci].getName() + ";";
			jTextField_Pipe.setText(pipeSting);
			
			setInfoMsg("Hashed pipe to: " + pipe.getPipeString());
			
		} catch (Exception e) {
			e.printStackTrace();
		}
	}
		
	protected void randomText_action(ActionEvent event) {
		String currentFortune = Fortune.getFortune();
		jTextAreaSource.setText(currentFortune);
		if (currentFortune.length() < 2048)
			jLabel_statusSource.setText(currentFortune.length() + " bytes");
		if (currentFortune.length()  > 2048 && currentFortune.length() < 1048576)
			jLabel_statusSource.setText((int)(currentFortune.length() / 1024) + " KB");
		if (currentFortune.length()> 1048576)
			jLabel_statusSource.setText((int)(currentFortune.length() / (1024*1024)) + " MB");
	}
	
	protected void resetForm_action(ActionEvent event) {		
		try {
			jTextAreaSource.setText("");
			jTextAreaDestination.setText("");
			jTextField_Pipe.setText("");
			jTextField_Hash.setText("");
			jTextField_Key.setText("zen@area23.at");
			// TODO: reset JComboBoxes jComboBox_Algo
			selectItemByString(jComboBox_Encoding, menuEncoding, "Base64");
			selectItemByString(jComboBox_Hash, menuHash, "Hex");
			selectItemByString(jComboBox_Zip, menuZip, "None");
			
			setInfoMsg("Form cleared.");
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
		saveFileName = "";
		try {
			dbgMsg(String.format("pipe.encrypt with key=%s, hash=%s, \nencode=%s keyHash=%s, zip=%s", 
			 	key, hashed, encodeType.getName(), keyHash.getName(), zipType.getName()), 4, false);

			if (plain != null && plain.length() > 0) {
		    	
				if (plain.length() < 2048)
					jLabel_statusSource.setText(plain.length() + " bytes");
				if (plain.length()  > 2048 && plain.length() < 1048576)
					jLabel_statusSource.setText((int)(plain.length() / 1024) + " KB");
				if (plain.length()> 1048576)
					jLabel_statusSource.setText((int)(plain.length() / (1024*1024)) + " MB");
				
				encrypted = pipe.encrpytTextGoRounds(plain, key, hashed, encodeType, zipType, keyHash);
			    jTextAreaDestination.setText(encrypted);
								
				setInfoMsg("source text encrypted");
				if (encrypted.length() < 2048)
					jLabel_statusDestination.setText(encrypted.length() + " bytes");
				if (encrypted.length() > 2048 && encrypted.length() < 1048576)
					jLabel_statusDestination.setText((int)(encrypted.length() / 1024) + " KB.");
				if (encrypted.length() > 1048576)
					jLabel_statusDestination.setText((int)(encrypted.length() / (1024*1024)) + " MB.");
            }
            if (openFileBytes != null  && openFileBytes.length > 0) {
				
                saveFileBytes = pipe.encryptEncodeBytes(openFileBytes, key, hashed, encodeType, zipType, keyHash);
                saveFileSuffix = "";
                saveFileSuffix += (pipe.getPipeString().length() > 0) ? "." + keyHash.getName() : "";
                saveFileSuffix += (zipType != ZipType.None) ? ".gz" : "";
                saveFileSuffix += (pipe.getPipeString().length() > 0) ?  "." + pipe.getPipeString() : "";
                saveFileSuffix += (encodeType != EncodeEnum.None) ? "." + encodeType.getName() : ".base64";
                saveFileName = openFileName + saveFileSuffix;       
                saveFileName = saveFileToTemp(saveFileName, saveFileBytes);
				
				jLabel_fileOut.setText(saveFileName); 
                
				if (saveFileBytes.length < 2048)
                    jLabel_statusDestination.setText(saveFileBytes.length + " bytes"); 
                if (saveFileBytes.length > 2048 && saveFileBytes.length < 1048576) 
                    jLabel_statusDestination.setText((int)(saveFileBytes.length / 1024) + " KB."); 
                if (saveFileBytes.length > 1048576) 
                    jLabel_statusDestination.setText((int)(saveFileBytes.length / (1024*1024)) + " MB.");

            }
		} catch (Exception ex) {
			ex.printStackTrace();
			setInfoMsg("Exception during encrypt.");
			// jTextAreaDestination.setText(ex.toString());
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
        saveFileName = "";
		try {
			dbgMsg(String.format("pipe.decrypt with key=%s, hash=%s, \nencode=%s keyHash=%s, zip=%s",
			        key, hashed, encodeType.getName(), keyHash.getName(), zipType.getName()), 4, true);
			
            if (encrypted != null && encrypted.length() > 0) {
                
				if (encrypted.length() < 2048)
					jLabel_statusSource.setText(encrypted.length() + " bytes");
				if (encrypted.length()  > 2048 && encrypted.length() < 1048576)
					jLabel_statusSource.setText((int)(encrypted.length() / 1024) + " KB");
				if (encrypted.length()> 1048576)
					jLabel_statusSource.setText((int)(encrypted.length() / (1024*1024)) + " MB");
				
				decrypted = pipe.decryptTextRoundsGo(encrypted, key, hashed, encodeType, zipType, keyHash);
			    jTextAreaDestination.setText(decrypted);
				
				if (decrypted.length() < 2048)
					jLabel_statusDestination.setText(decrypted.length() + " bytes");
				if (decrypted.length() > 2048 && decrypted.length() < 1048576)
					jLabel_statusDestination.setText((int)(encrypted.length() / 1024) + " KB.");
				if (decrypted.length() > 1048576)
					jLabel_statusDestination.setText((int)(decrypted.length() / (1024*1024)) + " MB.");
				
				setInfoMsg("source text decrypted");
            }
            if (openFileBytes != null && openFileBytes.length > 0) {
                saveFileBytes = pipe.decodeDecrpytBytes(openFileBytes, key, hashed, encodeType, zipType, keyHash);
                int ptCnt = 0;
                for (int ix = 0; ix < openFileName.length(); ix++) {
                    if (openFileName.charAt(ix) == '.') {
                        if (++ptCnt == 2) {
                            saveFileName = openFileName.substring(0, ix);
                            break;
                        }
                    }
                }
				saveFileName = saveFileToTemp(saveFileName, saveFileBytes);
				
				jLabel_fileOut.setText(saveFileName); 
                
				if (saveFileBytes.length < 2048)
                    jLabel_statusDestination.setText(saveFileBytes.length + " bytes"); 
                if (saveFileBytes.length > 2048 && saveFileBytes.length < 1048576) 
                    jLabel_statusDestination.setText((int)(saveFileBytes.length / 1024) + " KB."); 
                if (saveFileBytes.length > 1048576) 
                    jLabel_statusDestination.setText((int)(saveFileBytes.length / (1024*1024)) + " MB.");
                
					save_action();
            } 
		} catch (Exception ex) {
			// jTextAreaDestination.setText(ex.toString());
			ex.printStackTrace();
			setInfoMsg("Exception during decrypt.");
		}
	}
	
	
	protected void about_action(ActionEvent event) {
	
		jTextAreaSource.append("About menu clicked, event: " + event + "\n");
		
        try {
            if (new File("/eu/cqrxs/gui/cqrxs-eu.jpg").isFile())
			    cqrJDialog = new CqrJDialog("/eu/cqrxs/gui/cqrxs-eu.jpg");
            else if (new File("eu/cqrxs/gui/cqrxs-eu.jpg").isFile())
			    cqrJDialog = new CqrJDialog("eu/cqrxs/gui/cqrxs-eu.jpg");
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
	
	protected static void selectItemByString(JComboBox cb, JMenu m, String s) {
		for (int i=0; i<cb.getItemCount(); i++) {
			if (cb.getItemAt(i).toString().equals(s)) {
				cb.setSelectedIndex(i);				
				break;
			}
		}
		selectMenuItemByString(m, s);
	}

	protected static void selectMenuItemByString(JMenu m, String s) {
		
		for (int i = 0 ; i < m.getItemCount(); i++) {
			JMenuItem item = m.getItem(i);
			if (item.getText().equals(s)) 
				item.setBackground(selectionBg); // item.setEnabled(enable);
			else 
				item.setBackground(defaultMenuItemBg);
		}
		
		return;
	}	

    protected String saveFileToTemp(String fname, byte[] fbytes) {
        String temp = System.getenv("TEMP");
        if (temp.isEmpty()) 
            temp = System.getenv("TMP");
        if (temp.isEmpty()) 
            temp = System.getenv("temp");
        if (temp.isEmpty()) 
            temp = ".";
    
        String dirSep = (File.pathSeparatorChar == ':') ? "/" : "\\";
        String fonly = fname;
        int idx = 0;
        while ((idx = fonly.indexOf(dirSep)) > -1) {
            int len = fonly.length();
            fonly = fonly.substring(idx + 1, len -1);
        }

        String spath = temp + dirSep + fonly;
        dbgMsg("fname=" + fname + " fonly=" + fonly + " spath = " + spath, 1, true); 
        Path fpath = java.nio.file.Paths.get(spath);

         try { 
             if (fbytes != null && fbytes.length > 0) { 
                Files.write(fpath, fbytes); 
                dbgMsg("filea: " + fbytes.length + " bytes writtem.", 1, true);                
            } else 
                throw new java.lang.IllegalStateException("fbytes is null or len == 0"); 
        } catch (Exception ex) { 
            setInfoMsg("Exception during file save.");
        }
            
        return fonly;

    }

	protected void setInfoMsg(String msg) {
		jLabel_infoMessage.setText(msg);
	}

}
