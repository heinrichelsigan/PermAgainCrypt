package eu.cqrxs.gui;

// Source - https://stackoverflow.com/a/13597635
// Posted by MadProgrammer, modified by community. See post 'Timeline' for change history
// Retrieved 2026-02-10, License - CC BY-SA 3.0

import javax.imageio.ImageIO;
import javax.swing.*;
import java.awt.*;
import java.awt.datatransfer.DataFlavor;
import java.awt.dnd.*;
import java.awt.image.BufferedImage;
import java.awt.datatransfer.Transferable;
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

    private JLabel message;

    public DropPanel() {
        File file;
        boolean successLoadImage = false;
        try {
            file = new File("eu/cqrxs/gui/file.png");
            target = ImageIO.read(file);
            successLoadImage = true;
        } catch (IOException ex) {
            ex.printStackTrace();
            successLoadImage = false;
        }
        if (!successLoadImage) {
            try {
                file = new File("file.png");
                target = ImageIO.read(file);
            } catch (IOException ex) {
                ex.printStackTrace();
            }
        }

        setLayout(new GridBagLayout());
        message = new JLabel();
        message.setBounds(4, 4, 144, 36);
        message.setFont(message.getFont().deriveFont(Font.BOLD, 11));
        add(message);

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
        Runnable run = new Runnable() {
            @Override
            public void run() {
                message.setText("Drop: '" + droppedFile + "'.");
            }
        };
        SwingUtilities.invokeLater(run);
    }

    protected void importFiles(final List files) {
        Runnable run = new Runnable() {
            @Override
            public void run() {
                message.setText("You dropped " + files.size() + " files");
            }
        };
        SwingUtilities.invokeLater(run);
    }

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

            SwingUtilities.invokeLater(new DragUpdate(false, null));

            Transferable transferable = dtde.getTransferable();
            if (dtde.isDataFlavorSupported(DataFlavor.javaFileListFlavor)) {
                dtde.acceptDrop(dtde.getDropAction());
                try {

                    Object transferDataObjct = (Object) transferable.getTransferData(DataFlavor.javaFileListFlavor);
                    if (transferDataObjct instanceof List) {
                        List transferData = (List) transferDataObjct;
                        if (transferData != null &&
                                (transferData.getHeight() > 0) &&
                                (transferData.getWidth() > 0)) {
                            importFiles(transferData);
                            dtde.dropComplete(true);
                        }
                    }
                    if (transferDataObjct instanceof ArrayList<?> fileList) {
                        Object[] dropFilesStr = fileList.toArray();
                        if (dropFilesStr.length > 0) {
                            String ds = dropFilesStr[0].toString();
                            importFiles(ds);
                        }
                    }
                } catch (Exception ex) {
                    ex.printStackTrace();
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
