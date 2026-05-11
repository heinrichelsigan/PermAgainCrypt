/**
 * @author           <a href="mailto:heinrich.elsigan@cqrxs.eu">Heinrich Elsigan</a>
 * @version          V 2.26.428
 * @since            API 27 Oreo 8.1
 *
 * eu.cqrxs.gui.ImageHelper
 * Coded 2021-2033 by <a href="mailto:he@area23.at">Heinrich Elsigan</a>
 * <a href="https://heinrichelsigan.area23.at">heinrichelsigan.area23.at</a>
 */

package eu.cqrxs.util;

import eu.cqrxs.crypt.encoding.Base64Coder;

import java.awt.Graphics2D;
import java.awt.Image;
import java.awt.image.BufferedImage;
import java.awt.Toolkit;
import java.io.BufferedInputStream;
import java.io.InputStream;
import java.io.File;
import javax.imageio.ImageIO;
import javax.swing.ImageIcon;


public class ImgHlp {

    protected static ImgHlp helper = new ImgHlp();

    public ImgHlp() {

    }


    /**
     * getImageIcon gets an {@link ImageIcon} from included jar file or relative path location
     * @param jarImgPath{@link String[]} with path to image file inside or outside jar
     * @return {@link ImageIcon}
     */
    public ImageIcon getImageIcon(String jarImgPath) {
        ImageIcon imgIcon = null;
        try {
            imgIcon = new ImageIcon(getImage(jarImgPath));
        } catch (Exception e) {
            DbgWriter.msg("getImageIcon(String jarImgPath = " + jarImgPath + ") throwed Exception:", false);
            DbgWriter.msgex(e, false);
            imgIcon = new ImageIcon(getImageByFileNames(new String[] { jarImgPath }));
        }

        return imgIcon;
    }


    /**
     * getImage gets an {@link BufferedImage} from included jar file or relative path location
     * @param jarImgPath{@link String[]} with path to image file inside or outside jar
     * @return {@link BufferedImage}
     */
    public BufferedImage getImage(String jarImgPath) {
        BufferedImage bufimg = null;
        try {
            Image img = Toolkit.getDefaultToolkit().getImage(getClass().getResource(jarImgPath));
            bufimg = toBufferedImage(img);
        } catch (Exception e) {
            DbgWriter.msg("getJarImage(String jarImgPath = " + jarImgPath + ") throwed Exception:", false);
            DbgWriter.msgex(e, false);
            bufimg = getImageByFileNames(new String[] { jarImgPath });
        }

        return bufimg;
    }


    /*
     * setJarIncludedImage from Richard S. alias JJ Mr. Data
     * @param imgstr image String
     * @return {@link java.awt.Image}
     */
    public Image setJarIncludedImage(String imgstr) {
        Image img = null;
        try {
            InputStream is = getClass().getResourceAsStream(imgstr);
            BufferedInputStream bis = new BufferedInputStream(is);
            // a buffer large enough for our image can be byte[] byBuf = = new byte[is.available()];
            byte[] byBuf = new byte[262144];  // is.read(byBuf);  or something like that...
            int byteRead = bis.read(byBuf, 0, 262144);
            img = Toolkit.getDefaultToolkit().createImage(byBuf);
        } catch(Exception e) {
            DbgWriter.msg("setJarIncludedImage(String imgstr = " + imgstr + ") throwed Exception:", true);
            DbgWriter.msgex(e, true);
        }
        return img;
    }


    /**
     * getImageByFileNames
     * @param imagePaths Array {@link String[]} with possible image paths
     * @return {@link BufferedImage}
     */
    public BufferedImage getImageByFileNames(String[] imagePaths) {
        File file;
        BufferedImage bufimg = null;
        for (int fx = 0; fx < imagePaths.length; fx++) {
            try {
                file = new File(imagePaths[fx]);
                bufimg = ImageIO.read(file);
                fx = imagePaths.length - 1;
                break;
            } catch (Exception ex) {
                DbgWriter.msg("getImagesByFileNames(String[] imagePaths[" + fx + "] = " + imagePaths[fx] + ") throwed Exception:", true);
                DbgWriter.msgex(ex, true);
            }
            String shortImgPath = imagePaths[fx];
            if (imagePaths[fx].contains("img")) {
                int idx = imagePaths[fx].indexOf("img");
                int len = imagePaths[fx].length();
                shortImgPath = imagePaths[fx].substring(idx, len);
            }
            try {
                file = new File(shortImgPath);
                bufimg = ImageIO.read(file);
                fx = imagePaths.length - 1;
                break;
            } catch (Exception ex) {
                DbgWriter.msg("getImagesByFileNames(String[] imagePaths[" + fx + "] = " + imagePaths[fx]
                        + ") shortImgPath = " + shortImgPath + " throwed Exception:", true);
                DbgWriter.msgex(ex, true);
            }
        }
        return bufimg;
    }



    public static ImgHlp getHelper() {
        return helper;
    }



    /**
     * getJarImageIcon gets an {@link ImageIcon} from included jar file or relative path location
     * @param jarImgPath{@link String[]} with path to image file inside or outside jar
     * @return {@link ImageIcon}
     */
    public static ImageIcon getJarImageIcon(String jarImgPath) {
        return getHelper().getImageIcon(jarImgPath);
    }

    /**
     * getJarImageIcon gets an {@link BufferedImage} from included jar file or relative path location
     * @param jarImgPath{@link String[]} with path to image file inside or outside jar
     * @return {@link BufferedImage}
     */
    public static BufferedImage getJarImage(String jarImgPath) {
        return getHelper().getImage(jarImgPath);
    }


    /**
     * addImages
     * @param imagePaths Array {@link String[]} with possible image paths
     * @return {@link BufferedImage}
     */
    public static BufferedImage addImages(String[] imagePaths) {
        return getHelper().getImageByFileNames(imagePaths);
    }


    /*
	 * toBufferedImage converts {@link java.awt.Image} to {@link BufferedImage}
	 * @param img {@link java.awt.Image}
	 * @return {@link BufferedImage}
	 */
    public static BufferedImage toBufferedImage(Image img) {

		if (img instanceof BufferedImage)
            return (BufferedImage) img;

        // Create a buffered image with transparency
        BufferedImage bufimg = new BufferedImage(img.getWidth(null), img.getHeight(null), BufferedImage.TYPE_INT_ARGB);

        // Draw the image on to the buffered image
        Graphics2D bGr = bufimg.createGraphics();
        bGr.drawImage(img, 0, 0, null);
        bGr.dispose();

        // Return the buffered image
        return bufimg;
    }


    public static ImageIcon getKeyRing() {
        String base64 = "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAMAAABEpIrGAAAAwmVYSWZJSSoACAAAAAkAAAEEAAEAAAAgAAAAAQEEAAEAAAAgAAAAAgEDAAMAAAB6AAAAGgEFAAEAAACAAAAAGwEFAAEAAACIAAAAKAEDAAEAAAACAAAAMQECAAsAAACQAAAAMgECABQAAACcAAAAaYcEAAEAAACwAAAAAAAAAAgACAAIACwBAAABAAAALAEAAAEAAABHSU1QIDMuMC40AAAyMDI2OjAyOjIyIDE5OjI0OjM4AAEAAaADAAEAAAABAAAAAAAAAAscMsYAAAGEaUNDUElDQyBwcm9maWxlAAB4nH2Rv0vDQBzFX1O1IhVBi4g4ZKhOdlERx1qFIlQItUKrDiaX/hCaNCQpLo6Ca8HBH4tVBxdnXR1cBUHwB4h/gDgpukiJ30sKLWI8OO7Du3uPu3eAUC8zzeqIA5pum+lkQszmVsTQKwQMoh9dEGRmGbOSlILv+LpHgK93MZ7lf+7P0avmLQYEROI4M0ybeJ14etM2OO8TR1hJVonPicdNuiDxI9cVj984F10WeGbEzKTniCPEYrGNlTZmJVMjniKOqppO+ULWY5XzFmetXGXNe/IXhvP68hLXaY4giQUsQoIIBVVsoAwbMVp1UiykaT/h4x92/RK5FHJtgJFjHhVokF0/+B/87tYqTE54SeEE0PniOB+jQGgXaNQc5/vYcRonQPAZuNJb/kodmPkkvdbSokdA3zZwcd3SlD3gcgcYejJkU3alIE2hUADez+ibcsDALdCz6vXW3MfpA5ChrlI3wMEhMFak7DWfd3e39/bvmWZ/PyALcoVgnAn0AAANK2lUWHRYTUw6Y29tLmFkb2JlLnhtcAAAAAAAPD94cGFja2V0IGJlZ2luPSLvu78iIGlkPSJXNU0wTXBDZWhpSHpyZVN6TlRjemtjOWQiPz4KPHg6eG1wbWV0YSB4bWxuczp4PSJhZG9iZTpuczptZXRhLyIgeDp4bXB0az0iWE1QIENvcmUgNC40LjAtRXhpdjIiPgogPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4KICA8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIgogICAgeG1sbnM6eG1wTU09Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9tbS8iCiAgICB4bWxuczpzdEV2dD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL3NUeXBlL1Jlc291cmNlRXZlbnQjIgogICAgeG1sbnM6ZGM9Imh0dHA6Ly9wdXJsLm9yZy9kYy9lbGVtZW50cy8xLjEvIgogICAgeG1sbnM6R0lNUD0iaHR0cDovL3d3dy5naW1wLm9yZy94bXAvIgogICAgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIgogICB4bXBNTTpEb2N1bWVudElEPSJnaW1wOmRvY2lkOmdpbXA6M2E1OGZlNWItMzQzZC00YjAyLWJmMzUtY2ZhZWNhNDY5ZTA5IgogICB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOjBmMTFlNmY4LThjNmEtNGVjZi04ZDNiLTRjNjUyNjQyMTQyMyIKICAgeG1wTU06T3JpZ2luYWxEb2N1bWVudElEPSJ4bXAuZGlkOmZiZDcxODk0LWU5ZDctNGI3Mi1hMTkzLTIzYjRmZDVjNzQ2OCIKICAgZGM6Rm9ybWF0PSJpbWFnZS9wbmciCiAgIEdJTVA6QVBJPSIzLjAiCiAgIEdJTVA6UGxhdGZvcm09IkxpbnV4IgogICBHSU1QOlRpbWVTdGFtcD0iMTc3MTc4NDY4Nzg3NTk0NyIKICAgR0lNUDpWZXJzaW9uPSIzLjAuNCIKICAgeG1wOkNyZWF0b3JUb29sPSJHSU1QIgogICB4bXA6TWV0YWRhdGFEYXRlPSIyMDI2OjAyOjIyVDE5OjI0OjM4KzAxOjAwIgogICB4bXA6TW9kaWZ5RGF0ZT0iMjAyNjowMjoyMlQxOToyNDozOCswMTowMCI+CiAgIDx4bXBNTTpIaXN0b3J5PgogICAgPHJkZjpTZXE+CiAgICAgPHJkZjpsaQogICAgICBzdEV2dDphY3Rpb249InNhdmVkIgogICAgICBzdEV2dDpjaGFuZ2VkPSIvIgogICAgICBzdEV2dDppbnN0YW5jZUlEPSJ4bXAuaWlkOjU0N2Q5YTNhLTQzN2ItNDJhZi1hOTMxLTEzYWYyNzAwOGVlYSIKICAgICAgc3RFdnQ6c29mdHdhcmVBZ2VudD0iR0lNUCAzLjAuNCAoTGludXgpIgogICAgICBzdEV2dDp3aGVuPSIyMDI2LTAyLTIyVDE5OjI0OjQ3KzAxOjAwIi8+CiAgICA8L3JkZjpTZXE+CiAgIDwveG1wTU06SGlzdG9yeT4KICA8L3JkZjpEZXNjcmlwdGlvbj4KIDwvcmRmOlJERj4KPC94OnhtcG1ldGE+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAKPD94cGFja2V0IGVuZD0idyI/PgDnuSgAAADDUExURQAAAFhcXVpeX2BkZZiZmZObnNSZJJ6lp9ifN6Snp6apqqapq6epq9ujPamsrqmtrquur6yur6yvsK6xs6yztbG5ura5u/m2Nfm2Nti6gfm3Nvm4Oti9hvi6P9y/hvi8RMLDxMTFxcLGycHHycPIyuXFccbIycbIysXJy8TKzMXKzMXKzcbLzfnGU/7GScfMzsfMz8nMzsjNz8jN0MjO0MnO0crP0szP0svQ09TU1tTW19ve3u7v7/+goP+goP+goP+goNIp1qgAAAABdFJOUwBA5thmAAAAAWJLR0QAiAUdSAAAAAlwSFlzAAAuIwAALiMBeKU/dgAAAAd0SU1FB+oCFhIYL0LRG3kAAADsSURBVDjLrZLbUsJADIbLsglnsVQ5ploEBQwCKmd4/+diux2FoVl6Q64283+T/MnG8+4bEIdLJAQo8HbiIghDYOb1cjEAp64emOc8ZonAMFT5nOqbIsM3qQuqnFJqxEmkAKvCesUyYERlWsAvT12AaWFM/vwXAHmK58+h1QPHnO/wZPUo0h+uTcSrglKk9bdMfIX2L5qBk5ghxY+gKhOErwngNSpa70UXSAnhl7XeCEAPOwlgiUMaaBdf/hJfcErYqp0zgTAAXqSPhjimALrI64HepQG6cdOEXbylX3uQgcwKGR4yKpjDI+/+cQLHbRne+/DjLAAAAABJRU5ErkJggg==";
        return new ImageIcon((new Base64Coder()).decodeStringToBytes(base64));
    }

    public static ImageIcon getImgHash() {
        String base64 = "iVBORw0KGgoAAAANSUhEUgAAACAAAAAbCAMAAAAqGX2oAAAIqGVYSWZJSSoACAAAAAsAAAEEAAEAAAAgAAAAAQEEAAEAAAAbAAAAAgEDAAMAAACSAAAADgECABIAAACYAAAAEgEDAAEAAAABAAAAGgEFAAEAAACqAAAAGwEFAAEAAACyAAAAKAEDAAEAAAADAAAAMQECAAsAAAC6AAAAMgECABQAAADGAAAAaYcEAAEAAADaAAAAEgEAAAgACAAIAENyZWF0ZWQgd2l0aCBHSU1QAM4CAAATAAAAzgIAABMAAABHSU1QIDMuMC40AAAyMDI2OjAyOjIyIDE5OjIzOjA1AAIAhpIHABkAAAD4AAAAAaADAAEAAAABAAAAAAAAAAAAAAAAAAAAQ3JlYXRlZCB3aXRoIEdJTVAACQD+AAQAAQAAAAEAAAAAAQQAAQAAAAABAAABAQQAAQAAANgAAAACAQMAAwAAAIQBAAADAQMAAQAAAAYAAAAGAQMAAQAAAAYAAAAVAQMAAQAAAAMAAAABAgQAAQAAAIoBAAACAgQAAQAAAB4HAAAAAAAACAAIAAgA/9j/4AAQSkZJRgABAQAAAQABAAD/2wBDAAgGBgcGBQgHBwcJCQgKDBQNDAsLDBkSEw8UHRofHh0aHBwgJC4nICIsIxwcKDcpLDAxNDQ0Hyc5PTgyPC4zNDL/2wBDAQkJCQwLDBgNDRgyIRwhMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjL/wAARCADYAQADASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwDyyiiivrzwwooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAK2qxa2q0gRIKKKxacpWElc2qKxaKXOPlNqisWijnDlNqisWijnDlNqisWijnDlNqisWijnDlNqisWijnDlNqisWijnDlNqisWijnDlNqisWijnDlNqisWijnDlNqisWtqnGVxNWCsWtqiiUbgnYxaKKKyNAooooAK2qxa2q0gRIKxa2qxaJhEKKKKzLCiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACtqsWtqtIESCiiirJMWiiisDUKKKKACtqsWtqtIESCsWtqsWiYRCiiisywooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigArapKWtYxsZt3CiiiqEYtFFFYGoUUUUAFbVYtbVaQIkFYtbVYtEwiFFFbVTGNxt2MWtmsaihOw2rhRRRUjNmsaitmr+IjYxqKKKgsKKKKACtqisWtPhI3CiiisywooooAKKKKANmlrFrarWLuZtWCiiiqEYtFFFYGoUUUUAFbVYtbVaQIkFYtbVYtEwiFbVYtbVEAkYtFFFZlmzRWNRWnORyhRRRWZYUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAVtVi1tVpAiQUUUVZJi0UUVgahRRRQAVtVi1tVpAiQVi1tVi0TCIVtVi1tUQCRi0UUVmWFFFFABRRRQAUUUUAFFFFAGzWNRRVSlclKwUUUVJQUUUUAFFFFABW1WLW1WkCJBRRRVkmLRRRWBqFFFFABW1WLW1WkCJBWLW1WLRMIhRRRWZYUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAVtVi1tVpAiQUUUVZJi0UUVgahRRRQAVtVi1tVpAiQVi1tVi0TCIUUUVmWFFFFABRRRQAUUUUAFFFFABRRRQAUVtVi1Uo2JTuFFFFSUFFFFABRRRQAVtVi1tVpAiQUUUVZJi0UUVgahRRRQAVtVi1tVpAiQVi1tVi0TCIUUUVmWFFFFABRRRQAUUUUAFFFFABRRRQAUVtVi1Uo2JTuFFFFSUFFFFABRRRQAVtVi1tVpAiQUUUVZJi0UUVgahRRRQAVtVi1tVpAiQVi1tVi0TCIUUUVmWFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFbVYtbVaQIkFFFFWSYtFFFYGoUUUUAFbVYtbVaQIkFYtbVFOUbiTsYtFbVFLkHzGLRW1RRyBzGLRW1RRyBzGLRW1RRyBzGLRW1RRyBzGLRW1RRyBzGLRW1RRyBzGLRW1RRyBzGLRW1RRyBzGLRW1RRyBzGLW1RRTjGwm7hRRRVCMWiiisDUKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigD//2fHcKc4AAAGEaUNDUElDQyBwcm9maWxlAAB4nH2Rv0vDQBzFX1O1IhVBi4g4ZKhOdlERx1qFIlQItUKrDiaX/hCaNCQpLo6Ca8HBH4tVBxdnXR1cBUHwB4h/gDgpukiJ30sKLWI8OO7Du3uPu3eAUC8zzeqIA5pum+lkQszmVsTQKwQMoh9dEGRmGbOSlILv+LpHgK93MZ7lf+7P0avmLQYEROI4M0ybeJ14etM2OO8TR1hJVonPicdNuiDxI9cVj984F10WeGbEzKTniCPEYrGNlTZmJVMjniKOqppO+ULWY5XzFmetXGXNe/IXhvP68hLXaY4giQUsQoIIBVVsoAwbMVp1UiykaT/h4x92/RK5FHJtgJFjHhVokF0/+B/87tYqTE54SeEE0PniOB+jQGgXaNQc5/vYcRonQPAZuNJb/kodmPkkvdbSokdA3zZwcd3SlD3gcgcYejJkU3alIE2hUADez+ibcsDALdCz6vXW3MfpA5ChrlI3wMEhMFak7DWfd3e39/bvmWZ/PyALcoVgnAn0AAANcmlUWHRYTUw6Y29tLmFkb2JlLnhtcAAAAAAAPD94cGFja2V0IGJlZ2luPSLvu78iIGlkPSJXNU0wTXBDZWhpSHpyZVN6TlRjemtjOWQiPz4KPHg6eG1wbWV0YSB4bWxuczp4PSJhZG9iZTpuczptZXRhLyIgeDp4bXB0az0iWE1QIENvcmUgNC40LjAtRXhpdjIiPgogPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4KICA8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIgogICAgeG1sbnM6eG1wTU09Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9tbS8iCiAgICB4bWxuczpzdEV2dD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL3NUeXBlL1Jlc291cmNlRXZlbnQjIgogICAgeG1sbnM6ZGM9Imh0dHA6Ly9wdXJsLm9yZy9kYy9lbGVtZW50cy8xLjEvIgogICAgeG1sbnM6R0lNUD0iaHR0cDovL3d3dy5naW1wLm9yZy94bXAvIgogICAgeG1sbnM6dGlmZj0iaHR0cDovL25zLmFkb2JlLmNvbS90aWZmLzEuMC8iCiAgICB4bWxuczp4bXA9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8iCiAgIHhtcE1NOkRvY3VtZW50SUQ9ImdpbXA6ZG9jaWQ6Z2ltcDo0MDkyMWIxZi03NzA2LTQwODAtYjY0ZC1iMzc1ODEwNDBkZTQiCiAgIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6N2IyYWU1YjgtZDI5YS00OTFjLWFmZGUtNTlmMWFlYzgwZDUyIgogICB4bXBNTTpPcmlnaW5hbERvY3VtZW50SUQ9InhtcC5kaWQ6NzM0MjUxOGEtYTRkYy00YTI5LWJiZTAtMWRlMGJhNzc1NDdkIgogICBkYzpGb3JtYXQ9ImltYWdlL3BuZyIKICAgR0lNUDpBUEk9IjMuMCIKICAgR0lNUDpQbGF0Zm9ybT0iTGludXgiCiAgIEdJTVA6VGltZVN0YW1wPSIxNzcxNzg0NTk0NDM0NDgzIgogICBHSU1QOlZlcnNpb249IjMuMC40IgogICB0aWZmOk9yaWVudGF0aW9uPSIxIgogICB4bXA6Q3JlYXRvclRvb2w9IkdJTVAiCiAgIHhtcDpNZXRhZGF0YURhdGU9IjIwMjY6MDI6MjJUMTk6MjM6MDUrMDE6MDAiCiAgIHhtcDpNb2RpZnlEYXRlPSIyMDI2OjAyOjIyVDE5OjIzOjA1KzAxOjAwIj4KICAgPHhtcE1NOkhpc3Rvcnk+CiAgICA8cmRmOlNlcT4KICAgICA8cmRmOmxpCiAgICAgIHN0RXZ0OmFjdGlvbj0ic2F2ZWQiCiAgICAgIHN0RXZ0OmNoYW5nZWQ9Ii8iCiAgICAgIHN0RXZ0Omluc3RhbmNlSUQ9InhtcC5paWQ6M2M1NzlmMDMtMTA4MS00MDg3LThjOTAtOGEyYzk0ZTFjOTg4IgogICAgICBzdEV2dDpzb2Z0d2FyZUFnZW50PSJHSU1QIDMuMC40IChMaW51eCkiCiAgICAgIHN0RXZ0OndoZW49IjIwMjYtMDItMjJUMTk6MjM6MTQrMDE6MDAiLz4KICAgIDwvcmRmOlNlcT4KICAgPC94bXBNTTpIaXN0b3J5PgogIDwvcmRmOkRlc2NyaXB0aW9uPgogPC9yZGY6UkRGPgo8L3g6eG1wbWV0YT4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgIAo8P3hwYWNrZXQgZW5kPSJ3Ij8+ZKesuwAAAB5QTFRFAAAAADKLEkuxEkuyE0uwE0uxE0uyEkyxE0yxE0yy6DoduwAAAAF0Uk5TAEDm2GYAAAABYktHRACIBR1IAAAACXBIWXMAAA7DAAAOwwHHb6hkAAAAB3RJTUUH6gIWEhcOiSAX6AAAAHJJREFUKM+lklEKACEIRB2W/ej+F44yC9fMrSQK7DFOJlEcWAQDyQ0BXHEmfgEwxs4Atl13AwBvTUL2ktAKTaSfZX2Ah1sjJSwAUjpTABhOLNCupyW2Wg2Ef9ERDcgMpIEsPDBxrOB42HiFAYKpDcf+MjJvwAQo0C7HLQAAAABJRU5ErkJggg==";
        return new ImageIcon((new Base64Coder()).decodeStringToBytes(base64));
    }

    public static ImageIcon getCloseDelete() {
        String base64= "R0lGODlhGwAbAKUHAMzMzAAAAJmZmf/MzAAAZjMAAMyZmZjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2pjB2iH+EUNyZWF0ZWQgd2l0aCBHSU1QACH5BAGWAB8ALAAAAAAbABsAAAZnwI9wSCwaj8ikcslsOpGG6DJqSEYLhSoUKz0asFwvOLsda4dfs3VMFqbB5zL88w4z61l2fMpWP+l9bX+AfoN4dk+Hc054jU2OjkqRhItihW56coJEdXtoaZ5oXKFuXVamg6mqq6xPQQA7";
        return new ImageIcon((new Base64Coder()).decodeStringToBytes(base64));
    }

    public static ImageIcon getAesArrowHover() {
        String base64 = "R0lGODlhIAAbAIQCAP////9mZszMzJmZmQAAAGYAmZkzzJkzmZkAmWZmZv+ZmcwzAJkAzAAAZsxmZswAAMwzzMwzM8wAM8xmzMzMzMzMzMzMzMzMzMzMzMzMzMzMzMzMzMzMzMzMzMzMzMzMzCH/C05FVFNDQVBFMi4wAwEAAAAh/iFDcmVhdGVkIGJ5IGhlQGFyZWEyMy5hdCB3aXRoIEdJTVAAIfkEATIAHwAsAAAAACAAGwAABVwgII5kaZ5oqq5s675wLM90rQqCfQoEkbu4oHBIGPhavJ5yuWwIjL9bcUCtWqsi6Io3gGlT3Nh3V/QewdOrugsYo5LMOMHpBg/vuOJZJ0pG+X1/gIOEhYaHiIklIQAh+QQBMgAfACwAAAAAIAAbAAAFhyAgjmRQkmiqnqO5vqsryjAa3G2wzMFTvwKTSzj7pW64IUBpbAmKSmaTF3guqzwrQKCd1gQEQvfLLZvNhIHYCA673++GQj1WgQf4vF6vmK9hd32Cg4R9dIBphYqDhyuBi4qNjml7lXiGf0Bwm25ykoBnoVxpmV6TpaaOdamsra6vsLGys7QkIQAh+QQBMgAfACwAAAAAIAAbAAAFoiAgjmRQkmiqnqO5vqsryjAa3G2wzMFTvwKTSzj7pW64IUBpbAmKSmaTF3guqzzrSKCd1gQEQvfLLZvNhIHYCA673++GQj1WgQf4vF6vmK9hd32Cg4R9dIBphYqDhyuBi4qNjml7lXiGf0Bwm25ykoBnoVxpmTU0k6WmbHU2OEs8O0E3rIBEUS1TSLZQuEZYr0TAvb6zr1exSLRexssvSM0iIQAh+QQBMgAfACwAAAAAIAAbAAAFsyAgjuRRkmiqkgi7vq85ynB6IPQNiXr9Qq0WwCSk+UY43JC3BAiPvN1SGGRCkT3nQYpASEkCwRUqMBjE5LB6vTYMzseyeU6vK95oWHnA7/v9Cndwem6BhoeIgXiEA4mOh4sre4+PkZJuf5l8ioMvcnWgZoJ5emymYW6dMAFxZqSraasBrAC0swsAArMBrz66tcDAtMFHu6zHIsjJULzLysrEPrtitgG4urO9YyPD2yu73iIhACH5BAEyAB8ALAAAAAAgABsAAAWrICCO5FGSaKqSCLu+rznKcHog9A2Jev1CrRbAJKT5RjjckLcECI+83VIYZEKRveFBmkUJBFeowGAAi7/odNowKB/H5Lh8rmibYeOBfs/nK+pueGx/hIWGf3aCA4eMhYkreY2Nj5BsfZd6iIEvcHOeZIB3eGqkX2ybL081cKIxV18xSk48CFyyYRAmRExPqjU3Si0ywjNQtUy7U0hXN1uzQ8fQRmE21DW501chACH5BAEyAB8ALAAAAAAgABsAAAVhICCOZGmeaKqubOu+cCzPdN0Kgn0KhpG7uKBwaBj4br2kcmlQGH8q3mBKrVYVzmO0iO16v9jndgAue8UoqdmMThet8GlYm+Ix7812dMjHFek6AHZQgSI4hYiJiouMjY4mIQAh+QQBMgAfACwAAAAAIAAbAAAFYiAgjmRpnmiqrmzrvnAsz3S9CoJ9CoaRu7igcGgY+Fq8nnLJVBh/Kt5gSq1WFc5jtIjter/Y53YALnvFKKnZjE4XrfBpWJtKMu+9LDQ67OOKdDoASXuCg4WGiYqLjI2OjyMhADs=";
        return new ImageIcon((new Base64Coder()).decodeStringToBytes(base64));
    }

}
