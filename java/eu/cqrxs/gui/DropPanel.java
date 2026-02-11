package eu.cqrxs.gui;

// Source - https://stackoverflow.com/a/13597635
// Posted by MadProgrammer, modified by community. See post 'Timeline' for change history
// Retrieved 2026-02-10, License - CC BY-SA 3.0

import eu.cqrxs.util.DbgWriter;
import javax.imageio.ImageIO;
import javax.swing.*;
import java.awt.*;
import java.awt.datatransfer.DataFlavor;
import java.awt.event.*;
import java.awt.dnd.*;
import java.awt.image.BufferedImage;
import java.awt.datatransfer.Transferable;
import java.lang.*;
import java.net.*;
import java.util.*;
import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.TooManyListenersException;

public class DropPanel extends JPanel {

    private DropTarget dropTarget;
    private DropTargetHandler dropTargetHandler;
    private Point dragPoint;

    private boolean dragOver = false;
    private BufferedImage target;

    private JLabel message, jLabel_fileIn;
	private ImageViewer imInFile;
	private URL fileInUrl;
	private Font monoSpaced = new Font("Monospaced", Font.PLAIN, 10);
    
	
	public DropPanel() {
        
        target = addImages(new String[] { "eu/cqrxs/gui/file.png", "file.png" });

        setLayout(null);
        message = new JLabel();
        message.setBounds(0, 0, 168, 96);
        message.setFont(message.getFont().deriveFont(Font.BOLD, 11));
        add(message);
		try {
			fileInUrl = URI.create("https://area23.at/net/res/img/crypt/file.png").toURL();
			imInFile = new ImageViewer();
			imInFile.setImageURL(fileInUrl);
			imInFile.setBounds(4, 4 ,60, 60);
			// imInFile.addMouseListener(aSymMouse);			
			add(imInFile);
		} catch (Exception ex) {
			ex.printStackTrace();			
		}
		jLabel_fileIn = new JLabel();
		jLabel_fileIn.setFont(monoSpaced);
		jLabel_fileIn.setBounds(4, 68, 120, 24);
		jLabel_fileIn.setText("[No input file loaded]");
		add(jLabel_fileIn);
    }
	
	private BufferedImage addImages(String[] images) {
		File file;
		BufferedImage bimg = null;
		for (int fx = 0; fx < images.length; fx++) {
			try {
				file = new File(images[fx]);
				bimg = ImageIO.read(file);
				fx = images.length - 1;
				break;
			} catch (IOException ex) {
				ex.printStackTrace();				
			}
		}
		return bimg;
	}

    @Override
    public Dimension getPreferredSize() {
        return new Dimension(400, 400);
    }

    protected DropTarget getMyDropTarget() {
        if (dropTarget == null) {
            dropTarget = new DropTarget(this, DnDConstants.ACTION_COPY_OR_MOVE, null);
        }
        return dropTarget;
    }

    protected DropTargetHandler getDropTargetHandler() {
        if (dropTargetHandler == null) {
            dropTargetHandler = new DropTargetHandler();
        }
        return dropTargetHandler;
    }

    @Override
    public void addNotify() {
        super.addNotify();
        try {
            getMyDropTarget().addDropTargetListener(getDropTargetHandler());
        } catch (TooManyListenersException ex) {
            ex.printStackTrace();
        }
    }

    @Override
    public void removeNotify() {
        super.removeNotify();
        getMyDropTarget().removeDropTargetListener(getDropTargetHandler());
    }

    @Override
    protected void paintComponent(Graphics g) {
        super.paintComponent(g);
        if (dragOver) {
            Graphics2D g2d = (Graphics2D) g.create();
            g2d.setColor(new Color(0, 255, 0, 64));
            g2d.fill(new Rectangle(getWidth(), getHeight()));
            if (dragPoint != null && target != null) {
                int x = dragPoint.x - 12;
                int y = dragPoint.y - 12;
                g2d.drawImage(target, x, y, this);
            }
            g2d.dispose();
        }
    }

    protected void importFiles(final String droppedFile) {
		// message.setText("'" + droppedFile + "'");
		DbgWriter.msg("Dropped: " + droppedFile, true);
		jLabel_fileIn.setText(droppedFile);
        Runnable run = new Runnable() {
            @Override
            public void run() {
				jLabel_fileIn.setText(droppedFile);
                // message.setText("Drop: '" + droppedFile + "'.");
            }
        };
        SwingUtilities.invokeLater(run);
    }

    // protected void importFiles(final List files) {
    //     Runnable run = new Runnable() {
    //         @Override
    //         public void run() {
    //             message.setText("You dropped " + files.size() + " files");
    //         }
    //     };
    //     SwingUtilities.invokeLater(run);
    // }

    protected class DropTargetHandler implements DropTargetListener {

        protected void processDrag(DropTargetDragEvent dtde) {
            if (dtde.isDataFlavorSupported(DataFlavor.javaFileListFlavor)) {
                dtde.acceptDrag(DnDConstants.ACTION_COPY);
            } else {
                dtde.rejectDrag();
            }
        }

        @Override
        public void dragEnter(DropTargetDragEvent dtde) {
            processDrag(dtde);
            SwingUtilities.invokeLater(new DragUpdate(true, dtde.getLocation()));
            repaint();
        }

        @Override
        public void dragOver(DropTargetDragEvent dtde) {
            processDrag(dtde);
            SwingUtilities.invokeLater(new DragUpdate(true, dtde.getLocation()));
            repaint();
        }

        @Override
        public void dropActionChanged(DropTargetDragEvent dtde) {
        }

        @Override
        public void dragExit(DropTargetEvent dte) {
            SwingUtilities.invokeLater(new DragUpdate(false, null));
            repaint();
        }

        @Override
        public void drop(DropTargetDropEvent dtde) {

			boolean dcompleted = false;
            SwingUtilities.invokeLater(new DragUpdate(false, null));

            Transferable transferable = dtde.getTransferable();
            if (dtde.isDataFlavorSupported(DataFlavor.javaFileListFlavor)) {
                
				dtde.acceptDrop(dtde.getDropAction());				
				Object transferDataObject;
				
                try {         
					transferDataObject = (Object) transferable.getTransferData(DataFlavor.javaFileListFlavor);
					if (transferDataObject instanceof ArrayList<?> fileList) {
                        Object[] dropFilesStr = fileList.toArray();
                        if (dropFilesStr.length > 0) {
                            String ds = dropFilesStr[0].toString();
                            importFiles(ds);
							dcompleted = true;
							dtde.dropComplete(true);
                        }
                    }
				} catch (Exception exArrayList) {
                    exArrayList.printStackTrace();
                }
				
				if (!dcompleted) try {
					transferDataObject = (Object) transferable.getTransferData(DataFlavor.javaFileListFlavor);
					if (transferDataObject instanceof java.awt.List) {
						java.awt.List transferData = (java.awt.List) transferDataObject;
						if (transferData != null && (transferData.getItemCount() > 0)) {	
								
							String[] drpFilesStr = ((java.awt.List)transferData).getItems();
							if (drpFilesStr.length > 0) {
								String ds = drpFilesStr[0].toString();
								importFiles(ds);
								dcompleted = true;
								dtde.dropComplete(true);
							}
						}
					}
				} catch (Exception exList) {
					exList.printStackTrace();
				}
				
            } else {
                dtde.rejectDrop();
            }
        }
		
    }

   
    public class DragUpdate implements Runnable {

		private boolean dragOver;
		private Point dragPoint;

		public DragUpdate(boolean dragOver, Point dragPoint) {
			this.dragOver = dragOver;
			this.dragPoint = dragPoint;
		}

		@Override
		public void run() {
			DropPanel.this.dragOver = dragOver;
			DropPanel.this.dragPoint = dragPoint;
			DropPanel.this.repaint();
		}
	}
   
}
