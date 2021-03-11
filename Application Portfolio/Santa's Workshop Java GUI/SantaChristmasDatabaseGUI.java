/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package santachristmasdatabasegui;

import java.sql.Connection;
import java.sql.DriverManager;
import javax.swing.JOptionPane;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.swing.DefaultComboBoxModel;
import javax.swing.DefaultListModel;

/**
 *
 * @author redor
 */
public class SantaChristmasDatabaseGUI extends javax.swing.JFrame {

    /**
     * Creates new form SantaChristmasDatabaseGUI
     */
    
    DefaultComboBoxModel<Child> childModel = new DefaultComboBoxModel<Child>();
    DefaultComboBoxModel<Child> childActionModel = new DefaultComboBoxModel<Child>();//Make it so that both childComboBoxes don't change when one changes
    DefaultComboBoxModel<Child> addChildActionModel = new DefaultComboBoxModel<Child>();
    DefaultListModel<Toy> toyModel = new DefaultListModel<Toy>();
    DefaultListModel<String> actionModel = new DefaultListModel<String>();
    Connection SQLCnn;
    ResultSet result;
    Statement azureSt;
    
    public void connectDatabase() {
        this.setTitle("Connected");
        try{
            Connection SQLCnn = DriverManager.getConnection("jdbc:sqlserver://hissongptc.database.windows.net:1433;database=HissongPTCDatabase;user=RedOrion800@hissongptc;password={Mario!8500};encrypt=true;trustServerCertificate=false;hostNameInCertificate=*.database.windows.net;loginTimeout=30;");
            //JOptionPane.showMessageDialog(null, "SQL Server Connection Successfully.");
            azureSt = SQLCnn.createStatement();
            
        }
        catch(Exception ex) {
            JOptionPane.showMessageDialog(this, ex.getMessage());
            this.setTitle("Not connected");
            System.exit(1);
        }
    }
    
    public void fillDropDown() {
        Child aChild = new Child(-1, "Select", "a Child");
        childModel.addElement(aChild);
        childActionModel.addElement(aChild);
        addChildActionModel.addElement(aChild);
        
        String SQL = "select * from getAllChildren order by ChildID";
        
        connectDatabase();
        try{
            result = azureSt.executeQuery(SQL);
            while(result.next()) {
                aChild = new Child(result.getInt("ChildID"), result.getString("FirstName"), result.getString("LastName"));
                childModel.addElement(aChild);
                childActionModel.addElement(aChild);
                addChildActionModel.addElement(aChild);
            }
            azureSt.close();
        }
        catch(SQLException ex) {
            JOptionPane.showMessageDialog(this, ex.getMessage());
            return;
        }
    }
    
    public void fillList() {
        toyModel.clear();
        String SQL;
        
        Child picked = (Child) this.childWishListCombo.getSelectedItem();
        SQL = String.format("select toys.ProductName, toys.manufacturer, toys.price from toys join wishlist "
                + "on toys.ToyID = wishlist.ToyID join child "
                + "on wishlist.childID = child.childID "
                + "where child.childID = %d order by ProductName;", picked.getChildID());
        //System.out.println(SQL);
        connectDatabase();
        try{
            result = azureSt.executeQuery(SQL);
            while(result.next()) {
                Toy aToy = new Toy(result.getString("ProductName"), result.getString("Manufacturer"), result.getDouble("Price"));
                toyModel.addElement(aToy);
            }
            azureSt.close();
        }
        catch(SQLException ex) {
            JOptionPane.showMessageDialog(this, ex.getMessage());
            return;
        }
        
    }
    
    public void fillActionList() {
        actionModel.clear();
        String SQL;
        
        Child picked = (Child) this.childActionCombo.getSelectedItem();
        
        if(this.actionCheckBox.isSelected()) {
            SQL = String.format("exec showAllNiceActions %d", picked.getChildID());
        }
        else {
            SQL = String.format("exec showAllNaughtyActions %d, %d", picked.getChildID(), Integer.parseInt(String.valueOf(this.severityCombo.getSelectedItem())));
        }
        //System.out.println(SQL);
        connectDatabase();
        try{
            result = azureSt.executeQuery(SQL);
            //used to make sure there are no results
            if (result.next() == false) { 
                JOptionPane.showMessageDialog(this, "There are no results that meet the criteria");
                actionModel.addElement("There are no results that meet the criteria.");
            } 
            else { 
                do { 
                String offence = result.getString("actionDescription");
                actionModel.addElement(offence);
                }while (result.next());
            }
            azureSt.close();
        }
        catch(SQLException ex) {
            JOptionPane.showMessageDialog(this, ex.getMessage());
            return;
        }
        
    }
    
    public void writeRecord(){
        Child picked = (Child) this.addActionChildCombo.getSelectedItem();
        String SQL;
        
        if(this.addActionNiceCheckBox.isSelected()) {
            SQL = String.format("exec addAction '%d', '%s', null", picked.getChildID(), this.addActionDescriptionTextField.getText());
        }
        else {
            SQL = String.format("exec addAction '%d', '%s', '%d'", picked.getChildID(), this.addActionDescriptionTextField.getText(), Integer.parseInt(String.valueOf(this.addActionSeverityCombo.getSelectedItem())));
        }
        //System.out.println(Integer.parseInt(String.valueOf(this.addActionSeverityCombo.getSelectedItem())));
        connectDatabase();
        
        try {
            azureSt.execute(SQL);
            azureSt.close();
            this.addActionDescriptionTextField.setText("");
            JOptionPane.showMessageDialog(this, "Action added successfully!");
        } catch (SQLException ex) {
            JOptionPane.showMessageDialog(this, ex.getMessage());
        }
    }
    
    public SantaChristmasDatabaseGUI() {
        initComponents();
        this.childWishListCombo.setModel(childModel);
        this.childActionCombo.setModel(childActionModel);
        this.wishList.setModel(toyModel);
        this.actionList.setModel(actionModel);
        this.addActionChildCombo.setModel(addChildActionModel);
        fillDropDown();
    }

    /**
     * This method is called from within the constructor to initialize the form.
     * WARNING: Do NOT modify this code. The content of this method is always
     * regenerated by the Form Editor.
     */
    @SuppressWarnings("unchecked")
    // <editor-fold defaultstate="collapsed" desc="Generated Code">//GEN-BEGIN:initComponents
    private void initComponents() {

        jTabbedPane1 = new javax.swing.JTabbedPane();
        jPanel1 = new javax.swing.JPanel();
        childWishListCombo = new javax.swing.JComboBox<>();
        jScrollPane2 = new javax.swing.JScrollPane();
        wishList = new javax.swing.JList<>();
        jLabel2 = new javax.swing.JLabel();
        jPanel2 = new javax.swing.JPanel();
        childActionCombo = new javax.swing.JComboBox<>();
        actionCheckBox = new javax.swing.JCheckBox();
        severityCombo = new javax.swing.JComboBox<>();
        searchButton = new javax.swing.JButton();
        jScrollPane3 = new javax.swing.JScrollPane();
        actionList = new javax.swing.JList<>();
        severityLabel = new javax.swing.JLabel();
        jLabel4 = new javax.swing.JLabel();
        jPanel3 = new javax.swing.JPanel();
        jLabel1 = new javax.swing.JLabel();
        addActionChildCombo = new javax.swing.JComboBox<>();
        jLabel5 = new javax.swing.JLabel();
        addActionNiceCheckBox = new javax.swing.JCheckBox();
        addActionSeverityCombo = new javax.swing.JComboBox<>();
        jLabel6 = new javax.swing.JLabel();
        addActionDescriptionTextField = new javax.swing.JTextField();
        addActionButton = new javax.swing.JButton();

        setDefaultCloseOperation(javax.swing.WindowConstants.EXIT_ON_CLOSE);

        childWishListCombo.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                childWishListComboActionPerformed(evt);
            }
        });

        jScrollPane2.setViewportView(wishList);

        jLabel2.setFont(new java.awt.Font("Tahoma", 0, 18)); // NOI18N
        jLabel2.setText("Child");

        javax.swing.GroupLayout jPanel1Layout = new javax.swing.GroupLayout(jPanel1);
        jPanel1.setLayout(jPanel1Layout);
        jPanel1Layout.setHorizontalGroup(
            jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(jPanel1Layout.createSequentialGroup()
                .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addGroup(jPanel1Layout.createSequentialGroup()
                        .addGap(24, 24, 24)
                        .addComponent(childWishListCombo, javax.swing.GroupLayout.PREFERRED_SIZE, 198, javax.swing.GroupLayout.PREFERRED_SIZE))
                    .addGroup(jPanel1Layout.createSequentialGroup()
                        .addGap(105, 105, 105)
                        .addComponent(jLabel2)))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED, 69, Short.MAX_VALUE)
                .addComponent(jScrollPane2, javax.swing.GroupLayout.PREFERRED_SIZE, 592, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addGap(20, 20, 20))
        );
        jPanel1Layout.setVerticalGroup(
            jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(jPanel1Layout.createSequentialGroup()
                .addContainerGap()
                .addComponent(jScrollPane2, javax.swing.GroupLayout.DEFAULT_SIZE, 330, Short.MAX_VALUE)
                .addContainerGap())
            .addGroup(jPanel1Layout.createSequentialGroup()
                .addGap(29, 29, 29)
                .addComponent(jLabel2)
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addComponent(childWishListCombo, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addContainerGap(javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE))
        );

        jTabbedPane1.addTab("View", jPanel1);

        actionCheckBox.setText("Nice Action");
        actionCheckBox.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                actionCheckBoxActionPerformed(evt);
            }
        });

        severityCombo.setModel(new javax.swing.DefaultComboBoxModel<>(new String[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" }));

        searchButton.setText("Search");
        searchButton.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                searchButtonActionPerformed(evt);
            }
        });

        jScrollPane3.setViewportView(actionList);

        severityLabel.setFont(new java.awt.Font("Tahoma", 0, 18)); // NOI18N
        severityLabel.setHorizontalAlignment(javax.swing.SwingConstants.CENTER);
        severityLabel.setText("Severity");

        jLabel4.setFont(new java.awt.Font("Tahoma", 0, 18)); // NOI18N
        jLabel4.setText("Child");

        javax.swing.GroupLayout jPanel2Layout = new javax.swing.GroupLayout(jPanel2);
        jPanel2.setLayout(jPanel2Layout);
        jPanel2Layout.setHorizontalGroup(
            jPanel2Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(jPanel2Layout.createSequentialGroup()
                .addGroup(jPanel2Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addComponent(childActionCombo, javax.swing.GroupLayout.PREFERRED_SIZE, 172, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addGroup(jPanel2Layout.createSequentialGroup()
                        .addGap(32, 32, 32)
                        .addGroup(jPanel2Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                            .addComponent(severityCombo, javax.swing.GroupLayout.PREFERRED_SIZE, 91, javax.swing.GroupLayout.PREFERRED_SIZE)
                            .addComponent(actionCheckBox)
                            .addComponent(searchButton, javax.swing.GroupLayout.PREFERRED_SIZE, 91, javax.swing.GroupLayout.PREFERRED_SIZE)
                            .addComponent(severityLabel, javax.swing.GroupLayout.PREFERRED_SIZE, 91, javax.swing.GroupLayout.PREFERRED_SIZE)))
                    .addGroup(jPanel2Layout.createSequentialGroup()
                        .addGap(61, 61, 61)
                        .addComponent(jLabel4)))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED, 75, Short.MAX_VALUE)
                .addComponent(jScrollPane3, javax.swing.GroupLayout.PREFERRED_SIZE, 627, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addGap(29, 29, 29))
        );
        jPanel2Layout.setVerticalGroup(
            jPanel2Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(jPanel2Layout.createSequentialGroup()
                .addGap(32, 32, 32)
                .addGroup(jPanel2Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addGroup(jPanel2Layout.createSequentialGroup()
                        .addComponent(jScrollPane3, javax.swing.GroupLayout.PREFERRED_SIZE, 251, javax.swing.GroupLayout.PREFERRED_SIZE)
                        .addContainerGap(73, Short.MAX_VALUE))
                    .addGroup(jPanel2Layout.createSequentialGroup()
                        .addGap(22, 22, 22)
                        .addComponent(jLabel4)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.UNRELATED)
                        .addComponent(childActionCombo, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                        .addGap(26, 26, 26)
                        .addComponent(actionCheckBox)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.UNRELATED)
                        .addComponent(severityLabel, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.UNRELATED)
                        .addComponent(severityCombo, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                        .addGap(46, 46, 46)
                        .addComponent(searchButton)
                        .addGap(55, 55, 55))))
        );

        jTabbedPane1.addTab("Search", jPanel2);

        jLabel1.setFont(new java.awt.Font("Tahoma", 0, 18)); // NOI18N
        jLabel1.setText("Child");

        jLabel5.setFont(new java.awt.Font("Tahoma", 0, 18)); // NOI18N
        jLabel5.setText("Action Type and Severity");

        addActionNiceCheckBox.setText("Nice Action");
        addActionNiceCheckBox.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                addActionNiceCheckBoxActionPerformed(evt);
            }
        });

        addActionSeverityCombo.setModel(new javax.swing.DefaultComboBoxModel<>(new String[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" }));

        jLabel6.setFont(new java.awt.Font("Tahoma", 0, 18)); // NOI18N
        jLabel6.setText("Action Description");

        addActionButton.setText("Add");
        addActionButton.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                addActionButtonActionPerformed(evt);
            }
        });

        javax.swing.GroupLayout jPanel3Layout = new javax.swing.GroupLayout(jPanel3);
        jPanel3.setLayout(jPanel3Layout);
        jPanel3Layout.setHorizontalGroup(
            jPanel3Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(jPanel3Layout.createSequentialGroup()
                .addGroup(jPanel3Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addGroup(jPanel3Layout.createSequentialGroup()
                        .addGap(23, 23, 23)
                        .addComponent(addActionChildCombo, javax.swing.GroupLayout.PREFERRED_SIZE, 175, javax.swing.GroupLayout.PREFERRED_SIZE)
                        .addGap(178, 178, 178)
                        .addComponent(addActionNiceCheckBox)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED, 179, Short.MAX_VALUE))
                    .addGroup(jPanel3Layout.createSequentialGroup()
                        .addGap(94, 94, 94)
                        .addComponent(jLabel1)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                        .addComponent(jLabel5)
                        .addGap(128, 128, 128)))
                .addGroup(jPanel3Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addComponent(addActionDescriptionTextField, javax.swing.GroupLayout.Alignment.TRAILING, javax.swing.GroupLayout.PREFERRED_SIZE, 257, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addGroup(javax.swing.GroupLayout.Alignment.TRAILING, jPanel3Layout.createSequentialGroup()
                        .addComponent(jLabel6)
                        .addGap(60, 60, 60))))
            .addGroup(jPanel3Layout.createSequentialGroup()
                .addGap(394, 394, 394)
                .addGroup(jPanel3Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING, false)
                    .addComponent(addActionButton, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                    .addComponent(addActionSeverityCombo, 0, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE))
                .addContainerGap(javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE))
        );
        jPanel3Layout.setVerticalGroup(
            jPanel3Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(jPanel3Layout.createSequentialGroup()
                .addGap(51, 51, 51)
                .addGroup(jPanel3Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                    .addComponent(jLabel1)
                    .addComponent(jLabel5)
                    .addComponent(jLabel6))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.UNRELATED)
                .addGroup(jPanel3Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                    .addComponent(addActionChildCombo, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addComponent(addActionNiceCheckBox)
                    .addComponent(addActionDescriptionTextField, javax.swing.GroupLayout.PREFERRED_SIZE, 38, javax.swing.GroupLayout.PREFERRED_SIZE))
                .addGap(18, 18, 18)
                .addComponent(addActionSeverityCombo, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED, 142, Short.MAX_VALUE)
                .addComponent(addActionButton)
                .addGap(25, 25, 25))
        );

        jTabbedPane1.addTab("Add", jPanel3);

        javax.swing.GroupLayout layout = new javax.swing.GroupLayout(getContentPane());
        getContentPane().setLayout(layout);
        layout.setHorizontalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(layout.createSequentialGroup()
                .addContainerGap()
                .addComponent(jTabbedPane1)
                .addContainerGap())
        );
        layout.setVerticalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(javax.swing.GroupLayout.Alignment.TRAILING, layout.createSequentialGroup()
                .addContainerGap()
                .addComponent(jTabbedPane1)
                .addContainerGap())
        );

        pack();
    }// </editor-fold>//GEN-END:initComponents

    private void actionCheckBoxActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_actionCheckBoxActionPerformed
        if(this.actionCheckBox.isSelected()) {
            this.severityLabel.setVisible(false);
            this.severityCombo.setVisible(false);
        }
        else{
            this.severityCombo.setVisible(true);
            this.severityLabel.setVisible(true);
        }
    }//GEN-LAST:event_actionCheckBoxActionPerformed

    private void childWishListComboActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_childWishListComboActionPerformed
        fillList();
    }//GEN-LAST:event_childWishListComboActionPerformed

    private void searchButtonActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_searchButtonActionPerformed
        fillActionList();
    }//GEN-LAST:event_searchButtonActionPerformed

    private void addActionNiceCheckBoxActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_addActionNiceCheckBoxActionPerformed
        if(this.addActionNiceCheckBox.isSelected()) {
            this.addActionSeverityCombo.setVisible(false);
        }
        else {
            this.addActionSeverityCombo.setVisible(true);
        }
    }//GEN-LAST:event_addActionNiceCheckBoxActionPerformed

    private void addActionButtonActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_addActionButtonActionPerformed
        if(this.addActionDescriptionTextField.getText().trim().equals("")) {
            JOptionPane.showMessageDialog(this, "Please Add an Action Description.");
        }
        else {
            writeRecord();
            actionModel.clear();
        }
    }//GEN-LAST:event_addActionButtonActionPerformed

    /**
     * @param args the command line arguments
     */
    public static void main(String args[]) {
        /* Set the Nimbus look and feel */
        //<editor-fold defaultstate="collapsed" desc=" Look and feel setting code (optional) ">
        /* If Nimbus (introduced in Java SE 6) is not available, stay with the default look and feel.
         * For details see http://download.oracle.com/javase/tutorial/uiswing/lookandfeel/plaf.html 
         */
        try {
            for (javax.swing.UIManager.LookAndFeelInfo info : javax.swing.UIManager.getInstalledLookAndFeels()) {
                if ("Nimbus".equals(info.getName())) {
                    javax.swing.UIManager.setLookAndFeel(info.getClassName());
                    break;
                }
            }
        } catch (ClassNotFoundException ex) {
            java.util.logging.Logger.getLogger(SantaChristmasDatabaseGUI.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        } catch (InstantiationException ex) {
            java.util.logging.Logger.getLogger(SantaChristmasDatabaseGUI.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        } catch (IllegalAccessException ex) {
            java.util.logging.Logger.getLogger(SantaChristmasDatabaseGUI.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        } catch (javax.swing.UnsupportedLookAndFeelException ex) {
            java.util.logging.Logger.getLogger(SantaChristmasDatabaseGUI.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        }
        //</editor-fold>

        /* Create and display the form */
        java.awt.EventQueue.invokeLater(new Runnable() {
            public void run() {
                new SantaChristmasDatabaseGUI().setVisible(true);
            }
        });
    }

    // Variables declaration - do not modify//GEN-BEGIN:variables
    private javax.swing.JCheckBox actionCheckBox;
    private javax.swing.JList<String> actionList;
    private javax.swing.JButton addActionButton;
    private javax.swing.JComboBox<Child> addActionChildCombo;
    private javax.swing.JTextField addActionDescriptionTextField;
    private javax.swing.JCheckBox addActionNiceCheckBox;
    private javax.swing.JComboBox<String> addActionSeverityCombo;
    private javax.swing.JComboBox<Child> childActionCombo;
    private javax.swing.JComboBox<Child> childWishListCombo;
    private javax.swing.JLabel jLabel1;
    private javax.swing.JLabel jLabel2;
    private javax.swing.JLabel jLabel4;
    private javax.swing.JLabel jLabel5;
    private javax.swing.JLabel jLabel6;
    private javax.swing.JPanel jPanel1;
    private javax.swing.JPanel jPanel2;
    private javax.swing.JPanel jPanel3;
    private javax.swing.JScrollPane jScrollPane2;
    private javax.swing.JScrollPane jScrollPane3;
    private javax.swing.JTabbedPane jTabbedPane1;
    private javax.swing.JButton searchButton;
    private javax.swing.JComboBox<String> severityCombo;
    private javax.swing.JLabel severityLabel;
    private javax.swing.JList<Toy> wishList;
    // End of variables declaration//GEN-END:variables
}
