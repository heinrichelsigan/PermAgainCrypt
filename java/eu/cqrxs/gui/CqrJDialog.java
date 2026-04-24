/*
	CqrJDialog
	https://heinrichelsigan.area23.at
*/
package eu.cqrxs.gui;

import eu.cqrxs.util.Constants;

import java.awt.Window;
import java.awt.Color;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.FlowLayout;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;
import javax.imageio.ImageIO;
import javax.swing.*;


public class CqrJDialog extends JDialog { /* implements MouseListener  { */
    		
	JPanel jPanelCenter = new JPanel();
	JLabel jLabel;
	JButton jButtonExit = new JButton();
	JTextField jTextField = new JTextField();
	static Color backColor, bgColor;
	File file;
	BufferedImage img;
	ImageIcon icon;
	static final String NEWLINE = System.getProperty("line.separator");
	
	public CqrJDialog() throws IOException {
		
		String filename = "eu/cqrxs/gui/img/cqrxs-eu.jpg";
		file = new File(filename);
		img = ImageIO.read(file);
		
		setModal(true);
        Init();
	}
	
 	public CqrJDialog(String filename) throws IOException {
		
		if (filename == null || filename.length() == 0)			
			filename = "eu/cqrxs/gui/img/cqrxs-eu.jpg";
		file = new File(filename);
		img = ImageIO.read(file);
		
		setModal(true);
        Init();
	}

    public int showDialog(Window parent) {
        setLocationRelativeTo(parent);
        setVisible(true);
        return 0;
    }
	
	public void showDialog(JFrame parentJFrame) {
        setLocationRelativeTo(parentJFrame);
        setVisible(true);
        return ;
    }
	
	
	public void Init() {
		
		setLayout(null);
		setSize(800, 492);
		setResizable(false);
		setTitle("About CqrJd: cqrxs.eu");

		backColor = Color.decode("#04339d");
		bgColor = Color.BLUE;
		getContentPane().setBackground(backColor);
			
        icon = new ImageIcon(img);
		jPanelCenter.setBounds(12, 8, 772, 332);
		jPanelCenter.setLayout(new FlowLayout());
		jPanelCenter.setBackground(backColor);
		JLabel jLabel = new JLabel();
		jLabel.setIcon(icon);
		jPanelCenter.add(jLabel);
		getContentPane().add(jPanelCenter);
			
		jTextField = new JTextField();
		jTextField.setBounds(32, 408, 144, 27);
		jTextField.setText(Constants.APP_NAME + " " + Constants.VERSION);
		jTextField.setBackground(Color.WHITE);
		jTextField.setForeground(Color.BLACK);
		jTextField.setEditable(false);
		getContentPane().add(jTextField);

		jButtonExit = new JButton();
		jButtonExit.setText("Exit");
		jButtonExit.setBackground(Color.LIGHT_GRAY);
		jButtonExit.setForeground(Color.BLACK);
		jButtonExit.setBounds(668, 408, 96, 27);
		jButtonExit.setActionCommand("jexit");
		getContentPane().add(jButtonExit);
		// setVisible(true);
		try {
			setDefaultCloseOperation(JDialog.DISPOSE_ON_CLOSE);
		} catch (Exception ex) {
			ex.printStackTrace();
		}
		
		SymAction lSymAction = new SymAction();
		jButtonExit.addActionListener(lSymAction);
	}

	
	class SymAction implements ActionListener
	{
		public void actionPerformed(ActionEvent event)
		{
			Object object = event.getSource();
			if (object == jButtonExit)
				closeJDialog(event);
		}
	}

	public void closeJDialog(ActionEvent event) {
		// We don't log exit events ;)
		setDefaultCloseOperation(JDialog.DISPOSE_ON_CLOSE);
		this.removeAll();
		this.dispose();
		// System.exit(0);
	}

}
