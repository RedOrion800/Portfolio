/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package santachristmasdatabasegui;

import java.text.DecimalFormat;

/**
 *
 * @author redor
 */
public class Toy {
    String name;
    String manufacturer;
    double price;
    
    DecimalFormat money = new DecimalFormat("$###,###.00");

    public Toy() {
    }
    
    public Toy(String name, String manufacturer, double price) {
        this.name = name;
        this.manufacturer = manufacturer;
        this.price = price;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getManufacturer() {
        return manufacturer;
    }

    public void setManufacturer(String manufacturer) {
        this.manufacturer = manufacturer;
    }

    public double getPrice() {
        return price;
    }

    public void setPrice(double price) {
        this.price = price;
    }

    @Override
    public String toString() {
        return "Name: " + name + " | Made by " + manufacturer + " | Price: " + money.format(price) + "\n";
    }
    
    
}
