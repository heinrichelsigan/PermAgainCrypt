package eu.cqrxs.gui;

// Source - https://stackoverflow.com/a/13597635
// Posted by MadProgrammer, modified by community. See post 'Timeline' for change history
// Retrieved 2026-02-10, License - CC BY-SA 3.0

import eu.cqrxs.util.DbgWriter;

import java.awt.Color;
import java.awt.Font;
import java.awt.image.BufferedImage;
import java.awt.Point;

import java.awt.Graphics;
import java.awt.Dimension;
import java.awt.Graphics2D;
import java.awt.Rectangle;
import java.awt.datatransfer.DataFlavor;
import java.awt.dnd.DropTarget;
import java.awt.dnd.DropTargetEvent;
import java.awt.dnd.DropTargetDragEvent;
import java.awt.dnd.DropTargetDropEvent;
import java.awt.dnd.DropTargetListener;
import java.awt.dnd.DnDConstants;

import java.awt.image.BufferedImage;
import java.awt.datatransfer.Transferable;
import java.lang.*;
import java.net.*;
import java.util.*;
import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.TooManyListenersException;
import javax.imageio.ImageIO;
import javax.swing.ImageIcon;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.SwingUtilities;

public class DropPanel extends JPanel {

    private DropTarget dropTarget;
    private DropTargetHandler dropTargetHandler;
    private Point dragPoint;

    private boolean dragOver = false;
    private BufferedImage target, imgFileIn, imgFileOut, imgCipherPipe;
    private javax.swing.ImageIcon imgIconIn, imgIconOut, imgIconPipe;
    public JLabel message, jLabelImgIn, jLabelFileIn,  jLabelImgOut, jLabelFileOut, jLabelCipherPipe;
	private Font monoSpaced = new Font("Monospaced", Font.PLAIN, 10);
    public static CqrJFrameSimple jFrameSimple;
    public static CqrJdFrame jdFrame;

    public DropPanel(CqrJFrameSimple simple) {

        if (simple != null)
            jFrameSimple = simple;
        else
            jFrameSimple = (CqrJFrameSimple)getParent();

        setLayout(null);
        setSize(960, 108);
        initGui();
    }

    public DropPanel(CqrJdFrame complex) {

        if (complex != null)
            jdFrame = complex;
        else
            jdFrame = (CqrJdFrame)getParent();
        setLayout(null);
        setSize(960, 108);
        initGui();
    }


	public DropPanel() {

        setLayout(null);
        setSize(960, 108);
        initGui();
    }

    public void setCqrJdFrame(CqrJdFrame complex) {
        if (complex != null)
            jdFrame = complex;
        else
            jdFrame = (CqrJdFrame)getParent();

    }

    public void initGui() {
        try {

            target = addImages(new String[] { "eu/cqrxs/gui/file.png", "file.png" });
            imgFileIn  = addImages((new String[] {"eu/cqrxs/gui/file.png", "file.png" }));
            imgFileOut = addImages((new String[] {"eu/cqrxs/gui/encrypted.png", "encrypted.png" }));
			imgCipherPipe = addImages((new String[] {"eu/cqrxs/gui/cipherpipeblank.png", "cipherpipeblank.png" }));
			
            message = new JLabel();
            message.setBounds(0, 0, 742, 96);
            message.setFont(message.getFont().deriveFont(Font.BOLD, 11));
            add(message);

            jLabelImgIn = new JLabel(new ImageIcon(imgFileIn));
            jLabelImgIn.setBounds(4, 4, 60, 60);
            jLabelImgIn.setText("[No input file]");
            add(jLabelImgIn);

            jLabelFileIn = new JLabel();
            jLabelFileIn.setFont(monoSpaced);
            jLabelFileIn.setBounds(4, 68, 120, 24);
            jLabelFileIn.setText("[No input file loaded]");
            add(jLabelFileIn);

			jLabelCipherPipe = new JLabel(new ImageIcon(imgCipherPipe));
			jLabelCipherPipe.setBounds(108, 0, 640, 96);
            jLabelCipherPipe.setText("[blank cipher pipe]");
            add(jLabelCipherPipe);

            jLabelImgOut = new JLabel(new ImageIcon(imgFileOut));
            jLabelImgOut.setBounds(880, 4, 60, 60);
            add(jLabelImgOut);

            jLabelFileOut = new JLabel();
            jLabelFileOut.setFont(monoSpaced);
            jLabelFileOut.setBounds(880, 68, 120, 24);
            jLabelFileOut.setText("[No output file processed]");
            add(jLabelFileOut);

        } catch (Exception ex) {
            ex.printStackTrace();
        }
    }

    /**
     * scales an Image to 60x60 to be shown as thumbnail
     * @param bufImg {@link BufferedImage} to scale down to thumbnail size
     * @return thumbnail image
     */
    static public BufferedImage getThumbnail(BufferedImage bufImg) {
        int max_width = 60;
        int max_height = 60;
        int img_width = bufImg.getWidth();
        int img_height = bufImg.getHeight();

        float horizontal_ratio = 1;
        float vertical_ratio = 1;


        if (img_height > max_height) {
            vertical_ratio = (float) max_height / (float) img_height;
        }
        if (img_width > max_width) {
            horizontal_ratio = (float) max_width / (float) img_width;
        }

        float scale_ratio = 1;

        if (vertical_ratio < horizontal_ratio) {
            scale_ratio = vertical_ratio;
        } else if (horizontal_ratio <= vertical_ratio) {
            scale_ratio = horizontal_ratio;
        }

        int dest_width = (int) (img_width * scale_ratio);
        int dest_height = (int) (img_height * scale_ratio);

        BufferedImage scaled = new BufferedImage(dest_width, dest_height, BufferedImage.TYPE_INT_ARGB);
        Graphics graphics = scaled.getGraphics();
        graphics.drawImage(bufImg, 0, 0, dest_width, dest_height, null);
        graphics.dispose();

        return scaled;
    }


    /**
     * setPipeImg sets CipherPipe image
     * @param pipeImg image to set, if null reset to default image
     * @param pipeString pipeString
     */
    public void setPipeImg(BufferedImage pipeImg, String pipeString) {

        remove(jLabelCipherPipe);

        if (pipeImg == null) {
            jLabelCipherPipe = new JLabel(new ImageIcon(imgCipherPipe));
            jLabelCipherPipe.setBounds(108, 0, 640, 96);
            jLabelCipherPipe.setText("[blank cipher pipe]");
            add(jLabelCipherPipe);
        }  else {
            jLabelCipherPipe = new JLabel(new ImageIcon(pipeImg));
            jLabelCipherPipe.setBounds(108, 0, 640, 96);
            jLabelCipherPipe.setText(pipeString);
            add(jLabelCipherPipe);
        }
        repaint();
    }

    /**
     * addImages
     * @param imagePaths Array {@link String[]} with possible image paths
     * @return {@link BufferedImage}
     */
	private BufferedImage addImages(String[] imagePaths) {
		File file;
		BufferedImage bimg = null;
		for (int fx = 0; fx < imagePaths.length; fx++) {
			try {
				file = new File(imagePaths[fx]);
				bimg = ImageIO.read(file);
				fx = imagePaths.length - 1;
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

        DbgWriter.msg("Dropped: " + droppedFile, false);
        int ridx = droppedFile.lastIndexOf('/');
        if (ridx < 0)
            ridx = droppedFile.lastIndexOf('\\');
        String imgInName = (ridx > 0) ?
                droppedFile.substring(ridx + 1, droppedFile.length() - 1) :
                droppedFile;

        if (droppedFile.toLowerCase().endsWith(".jpg")  ||
                droppedFile.toLowerCase().endsWith(".jpeg")  ||
                droppedFile.toLowerCase().endsWith(".png")  ||
                droppedFile.toLowerCase().endsWith(".gif")  ||
                droppedFile.toLowerCase().endsWith(".bmp")  ||
                droppedFile.toLowerCase().endsWith(".tif"))
        {
            imgFileIn = addImages(new String[] { droppedFile });
            imgIconIn = new ImageIcon(getThumbnail(imgFileIn));
        }
        else {
            imgFileIn  = addImages((new String[] {"eu/cqrxs/gui/file.png", "file.png" }));
            imgIconIn = new ImageIcon(imgFileIn);
        }
        remove(jLabelImgIn);
        jLabelImgIn = new JLabel(imgIconIn);
        jLabelImgIn.setBounds(4, 4, 60, 60);
        add(jLabelImgIn);

        jLabelFileIn.setText(imgInName);
        jLabelImgOut.setVisible(false);
        jLabelFileOut.setVisible(false);
        // complex
        if (jdFrame != null)
            jdFrame.open_delegate(droppedFile);
        // simple
        if (jFrameSimple != null)
            jFrameSimple.open_delegate(droppedFile);

        /*
        Runnable run = new Runnable() {
            @Override
            public void run() {

                int ridx = droppedFile.lastIndexOf('/');
                if (ridx < 0)
                    ridx = droppedFile.lastIndexOf('\\');
                String imgInName = (ridx > 0) ?
                        droppedFile.substring(ridx + 1, droppedFile.length() - 1) :
                        droppedFile;

                if (droppedFile.toLowerCase().endsWith(".jpg")  ||
					droppedFile.toLowerCase().endsWith(".jpeg")  ||
					droppedFile.toLowerCase().endsWith(".png")  ||
					droppedFile.toLowerCase().endsWith(".gif")  ||
					droppedFile.toLowerCase().endsWith(".bmp")  ||
					droppedFile.toLowerCase().endsWith(".tif")) 
				{				
					imgFileIn = addImages(new String[] { droppedFile });
					imgIconIn = new ImageIcon(getThumbnail(imgFileIn));
				}
				else {
					imgFileIn  = addImages((new String[] {"eu/cqrxs/gui/file.png", "file.png" }));
					imgIconIn = new ImageIcon(imgFileIn);
				}
				remove(jLabelImgIn);
				jLabelImgIn = new JLabel(imgIconIn);
				jLabelImgIn.setBounds(4, 4, 60, 60);
				add(jLabelImgIn);

                jLabelFileIn.setText(imgInName);
				jLabelImgOut.setVisible(false);
				jLabelFileOut.setVisible(false);
                // complex
                if (jdFrame != null)
                    jdFrame.open_delegate(droppedFile);
                // simple
                if (jFrameSimple != null)
                    jFrameSimple.open_delegate(droppedFile);
                // ((CqrJFrameSimple)getParent()).open_delegate();
                // message.setText("Drop: '" + droppedFile + "'.");
            }
        };
        SwingUtilities.invokeLater(run);
        */
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

			DbgWriter.msg("drop(DropTargetDropEvent " + dtde.toString() + ") {...}", false);

            Transferable transferable = dtde.getTransferable();
            if (dtde.isDataFlavorSupported(DataFlavor.javaFileListFlavor)) {
                 				
				dtde.acceptDrop(dtde.getDropAction());				
				Object transferDataObject;
				
				try {
					transferDataObject = (Object) transferable.getTransferData(DataFlavor.javaFileListFlavor);
					if (transferDataObject instanceof java.awt.List) {
						DbgWriter.msg("transferDataObject instanceof java.awt.List", false);
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
					} else {						
						Object[] dropFilesStr = ((java.util.AbstractList)transferDataObject).toArray(); 
						// dropFilesStr = fileArray.asList<Object>().toArray();
						if (dropFilesStr.length > 0) {
							String ds = dropFilesStr[0].toString();
							importFiles(ds);
							dcompleted = true;
							dtde.dropComplete(true);
						}
					}
				} catch (Exception exList) {
					exList.printStackTrace();
				}
				
                if (!dcompleted) try {         
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
