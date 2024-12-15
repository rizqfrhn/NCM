from msvcrt import getch
import string
from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
from selenium.common.exceptions import TimeoutException
from selenium.webdriver.remote.webelement import WebElement
from selenium.webdriver.chrome.service import Service as ChromeService
from webdriver_manager.chrome import ChromeDriverManager
from selenium.webdriver.support.select import Select
import sys
import sqlite3
import time

# print string(sys.argv[1]) + int(sys.argv[2])

def updatedetailORU(channelconfig, essid, bridging, maccloning, iscloning, channelroam, delay, leavethreshold, scanthreshold, minsignal):
    try:
        sqliteConnection = sqlite3.connect('C:/Users/TRAKINDO/source/repos/NCM/db/NM.db')
        cursor = sqliteConnection.cursor()
        print("Connected to SQLite")

        sqlite_update_with_param = """UPDATE tb_oru SET channel = ?, essid = ?, bridging = ?, 
		mac_cloning = ?, iscloning = ?, channelroam = ?, delay = ?, leave_threshold = ?, scan_threshold = ?, min_signal = ? 
		WHERE no_loader = ?;"""
        data_tuple = (channelconfig, essid, bridging, maccloning, iscloning, channelroam, delay, leavethreshold, scanthreshold, minsignal, sys.argv[2])
        cursor.execute(sqlite_update_with_param, data_tuple)
        sqliteConnection.commit()
        print("Record Updated successfully")

        cursor.close()

    except sqlite3.Error as error:
        print("Failed to update data into sqlite table", error)
    finally:
        if sqliteConnection:
            sqliteConnection.close()
            print("The SQLite connection is closed")

# Variable for insert DB
channelconfig = string
essid = string
bridging = string
maccloning = string
iscloning = string
channelroam = string
delay = int
leavethreshold = int
scanthreshold = int
minsignal = int

options = webdriver.ChromeOptions()
options.headless = True

# Open ORU
print("Try open Chrome Webdriver")

def main():
	chrome_driver_path = 'C:/Users/TRAKINDO/Documents/Aby/chromedriver.exe'  # Replace with your path
	service = ChromeService(executable_path=chrome_driver_path)

	options = webdriver.ChromeOptions()
	options.headless = True

	with webdriver.Chrome(service=service, options=options) as browser:
		try:
			print("Try to open ORU " + sys.argv[1])
			browser.get('http://' + sys.argv[1])
			time.sleep(2)
			if sys.argv[1] != "":

				print("sampe sini ga sih?")
				
				# Click Setup Tab
				setuptab = browser.find_element(By.XPATH,'/html/body/div[2]/div/div[1]/ul/li[1]/a')
				setuptab.click()
				
				print("udah lewat setup tab nih")

				# Logic for Login
				# WebDriverWait(browser, 5).until(EC.presence_of_element_located((By.XPATH, '/html/body/div[2]/div/div[4]/form/div[2]/input[1]')))
				loginbtn = browser.find_element(By.XPATH,'/html/body/div[2]/div/div[4]/form/div[2]/input[1]')
					
				loginbtn.click()

				print("ini coba klik button login")
				
				try:
					if len(browser.find_element(By.XPATH,'/html/body/div[2]/div/div[4]/form/div[1]/div/div')) > 0:
						print("Go go Login with password!!")
						insertpass = browser.find_element(By.XPATH,'/html/body/div[2]/div/div[4]/form/div[1]/fieldset/fieldset/div[2]/div/input')
						insertpass.sendKeys("Minestar#1");
						loginbtn.click()
						browser.execute_script('window.open("")')
						time.sleep(3)
					else: 
						print("Go go Login without password!!")
						browser.execute_script('window.open("")')
						time.sleep(3)
				except:
					print('Aneh nih')
					pass
					
				# Select Wifi1 or Wifi2
				getchannel = browser.find_element(By.XPATH,'/html/body/div[2]/div/div[4]/form/div[1]/div[1]/fieldset/table/tbody/tr[3]/td[2]')
				wifi1btn = browser.find_element(By.XPATH,'/html/body/div[2]/div/div[4]/form/div[1]/div[1]/fieldset/table/tbody/tr[3]/td[7]/a[1]/img')
				wifi2btn = browser.find_element(By.XPATH,'/html/body/div[2]/div/div[4]/form/div[1]/div[2]/fieldset/table/tbody/tr[3]/td[7]/a[1]/img ')

				if getchannel == 6:
					wifi2btn.click()
					time.sleep(3)
					print("Open Wifi 2")
				else: 
					wifi1btn.click()
					time.sleep(3)
					print("Open Wifi 1")

				# Get channel from Device Config and essid from Interface Config
				print("coba get channel config")
				# getchannelconfig = Select(browser.find_element(By.XPATH,'/html/body/div[2]/div/div[4]/form/div[2]/fieldset[1]/div/div[2]/div[5]/div/select'))
				
				print("udah get channel config")
				getessid = browser.find_element(By.XPATH,'/html/body/div[2]/div/div[4]/form/div[2]/fieldset[2]/div/div[1]/div[3]/div/div/table/tbody/tr/td/input')
				
				print("udah dapet essid nih")
				
				# Go to Advanced Settings tab on Interface Config
				adsettab  = browser.find_element(By.XPATH,'/html/body/div[2]/div/div[4]/form/div[2]/fieldset[2]/ul/li[3]/a')
				adsettab.click()
				
				print("udah buka advanced setting")

				# getbridging = browser.find_element(By.XPATH,'/html/body/div[2]/div/div[4]/form/div[2]/fieldset[2]/div/div[4]/div[1]/div/select/')
				# getclonedmac = browser.find_element(By.XPATH,'/html/body/div[2]/div/div[4]/form/div[2]/fieldset[2]/div/div[4]/div[3]/div/div[1]/table/tbody/tr/td/input')
				
				print("udah buka advanced setting")
				
				try:
					if len(browser.find_element(By.XPATH,'/html/body/div[2]/div/div[4]/form/div[2]/fieldset[2]/div/div[4]/div[3]/div/div[1]/table/tbody/tr/td/input')) > 0:
						iscloning = 0;
						maccloning.append(browser.find_element(By.XPATH,'/html/body/div[2]/div/div[4]/form/div[2]/fieldset[2]/div/div[4]/div[3]/div/div[1]/table/tbody/tr/td/input'))
					else: 
						iscloning = 1;
				except:
					print('Aneh lagi nih')
					pass
				
				# Go to Roaming tab
				roamtab  = browser.find_element(By.XPATH,'/html/body/div[2]/div/div[4]/form/div[2]/fieldset[2]/ul/li[5]/a')
				roamtab.click()

				# getchannelroam = browser.find_element(By.XPATH,'/html/body/div[2]/div/div[4]/form/div[2]/fieldset[2]/div/div[8]/div[3]/div/select/')
				getdelay = browser.find_element(By.XPATH,'/html/body/div[2]/div/div[4]/form/div[2]/fieldset[2]/div/div[8]/div[4]/div/div[1]/table/tbody/tr/td/input')
				getleavethres = browser.find_element(By.XPATH,'/html/body/div[2]/div/div[4]/form/div[2]/fieldset[2]/div/div[8]/div[5]/div/div[1]/table/tbody/tr/td/input')
				getscanthres = browser.find_element(By.XPATH,'/html/body/div[2]/div/div[4]/form/div[2]/fieldset[2]/div/div[8]/div[7]/div/div[1]/table/tbody/tr/td/input')
				getminsignal = browser.find_element(By.XPATH,'/html/body/div[2]/div/div[4]/form/div[2]/fieldset[2]/div/div[8]/div[8]/div/div[1]/table/tbody/tr/td/input')
				
				print('Capeeee')

				# Input data from element to variable

				# channelconfig.append(getchannelconfig.text)
				print(getessid.get_attribute('value'))
				essid = getessid.get_attribute('value')
				print('Capeeee #2')
				# bridging.append(getbridging.text)
				# channelroam.append(getchannelroam.text)
				delay = getdelay.get_attribute('value')
				leavethreshold = getleavethres.get_attribute('value')
				scanthreshold = getscanthres.get_attribute('value')
				minsignal = getminsignal.get_attribute('value')
				
				print('Get Data ORU')
				
				# Insert data to DB
				updatedetailORU('', essid, '', '', '', '', delay, leavethreshold, scanthreshold, minsignal)
		except:
			print('Data Unavailable')

if __name__ == "__main__":
    main()