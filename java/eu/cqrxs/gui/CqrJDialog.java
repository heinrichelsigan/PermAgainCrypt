/*
	CqrJDialog
	https://heinrichelsigan.area23.at
*/
package eu.cqrxs.gui;

import java.awt.*;
import java.awt.event.*;
import java.awt.FlowLayout;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;
import java.lang.*;
import javax.imageio.ImageIO;
import javax.swing.ImageIcon;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.*;

public class CqrJDialog extends JDialog { /* implements MouseListener  { */
    		
	JPanel jPanelCenter = new JPanel();
	JLabel jLabel;
	JButton jButtonExit = new JButton();
	JTextField jTextField = new JTextField();
	File file;
	BufferedImage img;
	ImageIcon icon;
	static final String NEWLINE = System.getProperty("line.separator");
	
	public CqrJDialog() throws IOException {
		
		String filename = "eu/cqrxs/gui/cqrxs-eu.jpg";
		file = new File(filename);
		img = ImageIO.read(file);
		
		setModal(true);		
        setResizable(false);		
        setTitle("About CqrJd: cqrxs.eu");
		
        Init();
	}
	
 	public CqrJDialog(String filename) throws IOException {
		
		if (filename == null || filename.length() == 0)			
			filename = "eu/cqrxs/gui/cqrxs-eu.jpg";
		file = new File(filename);
		img = ImageIO.read(file);
		
		setModal(true);		
        setResizable(false);		
        setTitle("About CqrJd: cqrxs.eu");
		
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
		setSize(820, 600);			
			
        icon = new ImageIcon(img);
		jPanelCenter.setBounds(24, 20, 752, 400);
		jPanelCenter.setLayout(new FlowLayout());
		jPanelCenter.setBackground(Color.BLUE);  		
		JLabel jLabel = new JLabel();
		jLabel.setIcon(icon);
		jPanelCenter.add(jLabel);
		getContentPane().add(jPanelCenter);
			
		jTextField = new JTextField();
		jTextField.setBounds(144, 440, 600, 27);
		getContentPane().add(jTextField);
		jButtonExit = new JButton();
		jButtonExit.setText("Exit");
		getContentPane().add(jButtonExit);
		jButtonExit.setBounds(24, 440, 96, 27);
		jButtonExit.setActionCommand("jexit");
		// addMouseListener(this);
		// jPanelCenter.addMouseListener(this);
		
		// setVisible(true);
		try {
			setDefaultCloseOperation(JDialog.EXIT_ON_CLOSE);	
		} catch (Exception ex) { }
		
		SymAction lSymAction = new SymAction();
		jButtonExit.addActionListener(lSymAction);
	}

	
	class SymAction implements ActionListener
	{
		public void actionPerformed(ActionEvent event)
		{
			Object object = event.getSource();
					
			if (object == jButtonExit)
				appExit(event);				
		}
	}
	

	public void appExit(ActionEvent event) {
		// We don't log exit events ;)
		System.exit(0);
	}
	/*
	public void eventOutput(String eventDescription, MouseEvent e) {
        jTextField.setText(eventDescription + " detected on "
                + e.getComponent().getClass().getName()
                + ".");        
    }
     
    public void mousePressed(MouseEvent e) {
        eventOutput("Mouse pressed (# of clicks: "
                + e.getClickCount() + ")", e);
    }
     
    public void mouseReleased(MouseEvent e) {
        eventOutput("Mouse released (# of clicks: "
                + e.getClickCount() + ")", e);
    }
     
    public void mouseEntered(MouseEvent e) {
        eventOutput("Mouse entered", e);
    }
     
    public void mouseExited(MouseEvent e) {
        eventOutput("Mouse exited", e);
    }
     
    public void mouseClicked(MouseEvent e) {
        eventOutput("Mouse clicked (# of clicks: "
                + e.getClickCount() + ")", e);
    }
	*/
}
