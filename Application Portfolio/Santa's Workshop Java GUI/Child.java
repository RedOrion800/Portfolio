/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package santachristmasdatabasegui;

/**
 *
 * @author redor
 */
public class Child {
    int childID;
    String firstName;
    String lastName;

    public Child() {
    }

    public Child(int childID, String firstName, String lastName) {
        this.childID = childID;
        this.firstName = firstName;
        this.lastName = lastName;
    }

    public int getChildID() {
        return childID;
    }

    public void setChildID(int childID) {
        this.childID = childID;
    }

    public String getFirstName() {
        return firstName;
    }

    public void setFirstName(String firstName) {
        this.firstName = firstName;
    }

    public String getLastName() {
        return lastName;
    }

    public void setLastName(String lastName) {
        this.lastName = lastName;
    }

    @Override
    public String toString() {
        return firstName + " " + lastName;
    }
    
}
