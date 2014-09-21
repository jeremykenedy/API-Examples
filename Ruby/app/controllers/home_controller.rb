class HomeController < ApplicationController
  def index
  
	#Setup global request values
	site_ids = { 'int' => -99 }
	source_credentials = { 'SourceName' => 'YourSourceName', 'Password' => 'YourPassword', 'SiteIDs' => site_ids }
	user_credentials = { 'Username' => 'owner', 'Password' => 'demo1234', 'SiteIDs' => site_ids }
	
	
	#######################
	## Standard API call ##
	#######################
	
	#Create Savon client using default settings
	http_client = Savon.client("http://api.mindbodyonline.com/0_5/ClassService.asmx?WSDL")
	
	#Create request and package it for the call
	http_request = { 'SourceCredentials' => source_credentials, 'UserCredentials' => user_credentials  }
	params = { 'Request' => http_request }
	
	#Run the call and store the results
	result = http_client.request(:get_classes) do
		soap.body = params
	end
	
	@class_name = Array.new
	@class_description = Array.new
	@class_date = Array.new
	
	#Parse results for use in the view
	result[:get_classes_response][:get_classes_result][:classes][:class].each do |one_class|
		@class_name << one_class[:class_description][:name]
		@class_description << one_class[:class_description][:description]
		@class_date << "#{one_class[:start_date_time].strftime("%B %d, %Y %l:%M%P")} - #{one_class[:end_date_time].strftime("%B %d, %Y %l:%M%P")}"
	end
	
	
	############################
	## SSL protected API call ##
	############################
	
	#Create Savon client using a manually defined endpoint
	https_client = Savon::Client.new do |wsdl|
		wsdl.document = "http://api.mindbodyonline.com/0_5/SaleService.asmx?WSDL"
		wsdl.endpoint = "https://api.mindbodyonline.com/0_5/SaleService.asmx"
	end
	
	#Create request and package it for the call
	item = { 'ID' => 1364 }
	cart_item = { 'Quantity' => 1, 'Item' => item, :attributes! => { 'ins0:Item' => { 'xsi:type' => 'tns:Service' } } }
	cart_items = { 'CartItem' => cart_item }
	
	payment = { 'CreditCardNumber' => '4111111111111111', 'Amount' => 108.00, 'BillingAddress' => '123 Something', 'BillingCity' => 'SLO', 'BillingState' => 'CA', 'BillingPostalCode' => '93405', 'BillingName' => 'MindBody', 'ExpMonth' => '7', 'ExpYear' => '2016' } 
	payments = { 'PaymentInfo' => payment, :attributes! => { 'ins0:PaymentInfo' => { 'xsi:type' => 'tns:CreditCardInfo' } } }
	
	https_request = { 'SourceCredentials' => source_credentials, 'UserCredentials' => user_credentials, 'ClientID' => '002542', 'CartItems' => cart_items, 'Payments' => payments }
	params = { 'Request' => https_request }
	
	#Run the call and store the results
	result = https_client.request(:checkout_shopping_cart) do
		soap.body = params
	end
	
	#Parse results for use in the view
	@https_status = result[:checkout_shopping_cart_response][:checkout_shopping_cart_result][:status]
	
  end
end
